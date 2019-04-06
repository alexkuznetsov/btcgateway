using BTCGatewayAPI.Infrastructure.DB.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTCGatewayAPI.Infrastructure.DB
{
    public class MSSQLServerQueryBuilder : IQueryBuilder
    {
        public (StringBuilder, List<KeyValuePair<string, object>>) BuildInsertStatement(string id, object model)
        {
            var tableName = model.TableName();
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
                sql.AppendFormat("@{1},", p.Key, pName);
                parameters.Add(new KeyValuePair<string, object>(pName, p.Value));
            }

            sql = sql.Replace(",", "", sql.Length - 1, 1);
            sql.Append("); SELECT SCOPE_IDENTITY()");

            return (sql, parameters);
        }

        public (StringBuilder, List<KeyValuePair<string, object>>) BuildUpdateStatement(string id, object model)
        {
            var tableName = model.TableName();
            var dictionary = model.AsDictionary();
            var parameters = new List<KeyValuePair<string, object>>();
            var sql = new StringBuilder("UPDATE ");

            sql.AppendFormat(" [{0}] SET ", tableName);

            foreach (var p in dictionary.Where(x => x.Key != id))
            {
                var pName = $"p{p.Key}";
                sql.AppendFormat("{0} = @{1},", p.Key, pName);
                parameters.Add(new KeyValuePair<string, object>(pName, p.Value));
            }

            sql = sql.Replace(",", "", sql.Length - 1, 1);
            sql.AppendFormat(" WHERE {0} = @p{1}", id, id);

            parameters.Add(new KeyValuePair<string, object>("p" + id, dictionary[id]));

            return (sql, parameters);
        }

        public (StringBuilder, List<KeyValuePair<string, object>>) BuildDeleteStatement(string id, object model, object value)
        {
            var tableName = model.TableName();
            var parameters = new List<KeyValuePair<string, object>>();
            var sql = new StringBuilder("DELETE ");

            sql.AppendFormat("FROM [{0}] ", tableName);

            sql = sql.Replace(",", "", sql.Length - 1, 1);
            sql.AppendFormat(" WHERE {0} = @p{1}", id, id);

            parameters.Add(new KeyValuePair<string, object>("p" + id, value));

            return (sql, parameters);
        }
    }
}