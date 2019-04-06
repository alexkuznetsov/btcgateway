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

        public async Task<DbTransaction> BeginTransaction()
        {
            return await BeginTransaction(System.Data.IsolationLevel.Serializable);
        }

        public async Task<DbTransaction> BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            transaction = connection.BeginTransaction(isolationLevel);

            return transaction;
        }

        public async Task<IEnumerable<TModel>> GetMany<TModel>(string sql, params KeyValuePair<string, object>[] parameters)
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
                            if (conf.LogSQL)
                            {
                                Logger.Debug(msg);
                            }

                            throw new ObjectMapperExeption(msg);
                        }

                        readed.Add(mapSpec.Map(reader));
                    }
                }
            }

            //connection.Close();

            return readed;

        }

        public async Task<TModel> Find<TModel>(string sql, params KeyValuePair<string, object>[] parameters)
            where TModel : class, new()
        {
            var list = await GetMany<TModel>(sql, parameters);
            return list.SingleOrDefault();
        }

        /// <summary>
        /// Добавить новую запись
        /// TODO Ускорить выполнение метода, добавив кэширования метаданных
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TModel> Add<TModel>(TModel model)
            where TModel : class, new()
        {
            var id = model.GetIdField();
            var stmt = queryBuilder.BuildInsertStatement(id.Item1, model);
            var idValue = await ExecuteScalar(stmt.Item1, stmt.Item2.ToArray());

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
        public async Task<TModel> Update<TModel>(TModel model)
            where TModel : class, new()
        {
            var id = model.GetIdField();
            var stmt = queryBuilder.BuildUpdateStatement(id.Item1, model);
            var affected = await ExecuteNonQuery(stmt.Item1, stmt.Item2.ToArray());

            return model;
        }

        public async Task<int> Delete<TModel>(TModel model)
            where TModel : class, new()
        {
            var id = model.GetIdField();
            var idValue = id.Item2.GetValue(model);
            var stmt = queryBuilder.BuildDeleteStatement(id.Item1, model, idValue);
            var affected = await ExecuteNonQuery(stmt.Item1, stmt.Item2.ToArray());

            return affected;
        }

        private async Task<int> ExecuteNonQuery(StringBuilder stringBuilder, params KeyValuePair<string, object>[] parameters)
        {
            return await ExecuteMethod(stringBuilder
                , async (command) => await command.ExecuteNonQueryAsync().ConfigureAwait(false)
                , parameters);
        }

        private async Task<int> ExecuteScalar(StringBuilder stringBuilder, params KeyValuePair<string, object>[] parameters)
        {
            return await ExecuteMethod(stringBuilder, async (command) =>
            {
                var result = await command.ExecuteScalarAsync();

                return Convert.ToInt32(result);
            }, parameters);
        }

        private async Task<int> ExecuteMethod(StringBuilder stringBuilder, Func<DbCommand, Task<int>> executor, params KeyValuePair<string, object>[] parameters)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            var returnValue = 0;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = stringBuilder.ToString();
                command.Transaction = transaction;
                command.FillParameters(parameters);

                if (conf.LogSQL)
                {
                    Logger.Debug(command.CommandText);
                }

                returnValue = await executor(command);
            }

            //connection.Close();

            return returnValue;
        }
    }
}