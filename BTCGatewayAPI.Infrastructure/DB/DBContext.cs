using BTCGatewayAPI.Infrastructure.DB.Extensions;
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

        private bool disposed;
        private DbTransaction transaction;
        private readonly DbConnection connection;
        private readonly MapSpecRegistry registry;

        public DBContext(MapSpecRegistry registry, DbProviderFactory factory, string connStr)
        {
            this.registry = registry;

            connection = factory.CreateConnection();
            connection.ConnectionString = connStr;
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

        public DbTransaction BeginTransaction()
        {
            return BeginTransaction(System.Data.IsolationLevel.Serializable);
        }

        public DbTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            if (transaction == null)
            {
                transaction = connection.BeginTransaction(isolationLevel);
            }

            return transaction;
        }

        public async Task<IEnumerable<TModel>> GetMany<TModel>(string sql, params KeyValuePair<string, object>[] parameters)
            where TModel : class, new()
        {
            var readed = new List<TModel>();
            var mapSpec = registry.GetService<TModel>();

            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Transaction = transaction;

                foreach (var pValue in parameters)
                {
                    var p = command.CreateParameter();
                    p.ParameterName = pValue.Key;
                    p.Value = pValue.Value;
                    command.Parameters.Add(p);
                }

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var isInvalid = mapSpec.IsInvalid(reader);
                        if (isInvalid.Item1)
                        {
                            var errorColumns = isInvalid.Item2;
                            continue;
                            //TODO throw ObjectMapperExeption
                        }

                        readed.Add(mapSpec.Map(reader));
                    }
                }
            }

            connection.Close();

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
            var tableName = model.TableName();
            var id = model.GetIdField();
            var parameters = new List<KeyValuePair<string, object>>();
            var dictionary = model.AsDictionary();
            var sql = new StringBuilder("INSERT INTO ");
            sql.AppendFormat(" [{0}]", tableName);
            sql.Append('(');

            foreach (var f in dictionary.Where(x => x.Key != id).ToArray())
            {
                sql.AppendFormat("{0},", f.Key);
            }

            sql = sql.Replace(",", "", sql.Length - 1, 1);
            sql.Append(") VALUES (");

            foreach (var p in dictionary.Where(x => x.Key != id))
            {
                var pName = $"p{p.Key}";
                sql.AppendFormat("@p{1},", p.Key, pName);
                parameters.Add(new KeyValuePair<string, object>(pName, p.Value));
            }

            sql = sql.Replace(",", "", sql.Length - 1, 1);
            sql.AppendFormat("); SELECT SCOPE_IDENTITY()", id, id);

            parameters.Add(new KeyValuePair<string, object>("p" + id, dictionary[id]));

            var idValue = await ExecuteScalar(sql, parameters.ToArray());
            var idParam = model.GetType().GetProperty(id);

            idParam.SetValue(model, idValue);

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
            var tableName = model.TableName();
            var id = model.GetIdField();
            var parameters = new List<KeyValuePair<string, object>>();

            var updateSQL = new StringBuilder("UPDATE ");
            updateSQL.AppendFormat(" [{0}] SET", tableName);
            var dictionary = model.AsDictionary();

            foreach (var p in dictionary.Where(x => x.Key != id))
            {
                var pName = $"p{p.Key}";
                updateSQL.AppendFormat("{0} = @p{1},", p.Key, pName);
                parameters.Add(new KeyValuePair<string, object>(pName, p.Value));
            }

            updateSQL = updateSQL.Replace(",", "", updateSQL.Length - 1, 1);
            updateSQL.AppendFormat(" WHERE {0} = @p{1}", id, id);

            parameters.Add(new KeyValuePair<string, object>("p" + id, dictionary[id]));

            var affected = await ExecuteNonQuery(updateSQL, parameters.ToArray());

            return model;
        }

        private async Task<int> ExecuteNonQuery(StringBuilder stringBuilder, params KeyValuePair<string, object>[] parameters)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            var affected = 0;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = stringBuilder.ToString();
                command.Transaction = transaction;

                foreach (var pValue in parameters)
                {
                    var p = command.CreateParameter();
                    p.ParameterName = pValue.Key;
                    p.Value = pValue.Value;
                    command.Parameters.Add(p);
                }

                affected = await command.ExecuteNonQueryAsync();
            }

            connection.Close();

            return affected;
        }

        private async Task<int> ExecuteScalar(StringBuilder stringBuilder, params KeyValuePair<string, object>[] parameters)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            var value = 0;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = stringBuilder.ToString();
                command.Transaction = transaction;

                foreach (var pValue in parameters)
                {
                    var p = command.CreateParameter();
                    p.ParameterName = pValue.Key;
                    p.Value = pValue.Value;
                    command.Parameters.Add(p);
                }

                var result = await command.ExecuteScalarAsync();

                value = Convert.ToInt32(result);
            }

            connection.Close();

            return value;
        }
    }
}