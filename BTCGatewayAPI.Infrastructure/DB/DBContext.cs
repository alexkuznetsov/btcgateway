using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Infrastructure.DB
{
    public class DBContext : IDisposable
    {
        public const string DefaultProviderName = "System.Data.SqlClient";

        private static readonly Lazy<Logging.ILogger> LoggerLazy = new Lazy<Logging.ILogger>(Logging.LoggerFactory.GetLogger);

        private static Logging.ILogger Logger => LoggerLazy.Value;

        private bool disposed;
        private DbTransaction transaction;
        private readonly DbConnection connection;
        private readonly MapSpecRegistry registry;
        private readonly IQueryBuilder queryBuilder;
        private readonly GlobalConf conf;

        public DBContext(MapSpecRegistry registry, IQueryBuilder queryBuilder, GlobalConf conf, DbProviderFactory factory)
        {
            this.registry = registry;
            this.queryBuilder = queryBuilder;
            this.conf = conf;
            connection = factory.CreateConnection();
            connection.ConnectionString = conf.ConnectionString.ConnectionString;
        }

        #region IDisposable

        ~DBContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
                disposed = true;
            }
        }

        #endregion

        public async Task<DbTransaction> BeginTransactionAsync(System.Data.IsolationLevel isolationLevel)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            transaction = connection.BeginTransaction(isolationLevel);

            return transaction;
        }

        public async Task<IEnumerable<TModel>> GetManyAsync<TModel>(string sql, params KeyValuePair<string, object>[] parameters)
            where TModel : class, new()
        {
            var readed = new List<TModel>();
            var mapSpec = registry.GetService<TModel>();

            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            if (conf.LogSQL)
            {
                Logger.Debug(sql);
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Transaction = transaction;
                command.FillParameters(parameters);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var isInvalid = mapSpec.IsInvalid(reader);

                        if (isInvalid.Item1)
                        {
                            var msg = $"Check object mapping. Fail to map: " + string.Join(", ", isInvalid.Item2);

                            Logger.Error(msg);

                            throw new ObjectMapperException(msg);
                        }

                        readed.Add(mapSpec.Map(reader));
                    }
                }
            }

            //connection.Close();

            return readed;

        }

        public async Task<TModel> FindAsync<TModel>(string sql, params KeyValuePair<string, object>[] parameters)
            where TModel : class, new()
        {
            var list = await GetManyAsync<TModel>(sql, parameters);

            return list.SingleOrDefault();
        }

        /// <summary>
        /// Добавить новую запись
        /// TODO Ускорить выполнение метода, добавив кэширования метаданных
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TModel> AddAsync<TModel>(TModel model)
            where TModel : class, new()
        {
            var id = model.GetIdField();
            var stmt = queryBuilder.BuildInsertStatement(id.Item1, model);
            var idValue = await ExecuteScalarAsync(stmt.Item1, stmt.Item2.ToArray());

            id.Item2.SetValue(model, idValue);

            return model;
        }

        /// <summary>
        /// Обновить запись в БД
        /// TODO Ускорить выполнение метода, добавив кэширования метаданных
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TModel> UpdateAsync<TModel>(TModel model)
            where TModel : class, new()
        {
            var id = model.GetIdField();
            var stmt = queryBuilder.BuildUpdateStatement(id.Item1, model);
            var affected = await ExecuteNonQueryAsync(stmt.Item1, stmt.Item2.ToArray());

            return model;
        }

        public async Task<int> DeleteAsync<TModel>(TModel model)
            where TModel : class, new()
        {
            var id = model.GetIdField();
            var idValue = id.Item2.GetValue(model);
            var stmt = queryBuilder.BuildDeleteStatement(id.Item1, model, idValue);
            var affected = await ExecuteNonQueryAsync(stmt.Item1, stmt.Item2.ToArray());

            return affected;
        }

        private Task<int> ExecuteNonQueryAsync(StringBuilder sqlBuilder, params KeyValuePair<string, object>[] parameters)
            => ExecuteMethodAsync(sqlBuilder, (command) => command.ExecuteNonQueryAsync(), parameters);

        private Task<object> ExecuteScalarAsync(StringBuilder stringBuilder, params KeyValuePair<string, object>[] parameters)
            => ExecuteMethodAsync(stringBuilder, (command) => command.ExecuteScalarAsync(), parameters);

        private async Task<T> ExecuteMethodAsync<T>(StringBuilder stringBuilder, Func<DbCommand, Task<T>> executor, params KeyValuePair<string, object>[] parameters)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = stringBuilder.ToString();
                command.Transaction = transaction;
                command.FillParameters(parameters);

                if (conf.LogSQL)
                {
                    Logger.Debug(command.CommandText);
                }

                return await executor(command);
            }
        }
    }
}