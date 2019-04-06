using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace BTCGatewayAPI.Infrastructure
{
    internal static class ObjectExtensions
    {
        public static string TableName(this object model)
        {
            return model.GetType().GetAttr<TableAttribute>()?.Name;
        }

        public static (string, PropertyInfo) GetIdField(this object model)
        {
            return model.GetType().GetProperties()
                .Where(x => x.GetAttr<KeyAttribute>() != null)
                .Select(x => (x.GetSQLColumnName(), x))
                .SingleOrDefault();
        }

        private static string GetSQLColumnName(this MemberInfo member)
        {
            return member.GetAttr<ColumnAttribute>().Name;
        }

        public static Dictionary<string, object> AsDictionary(this object model)
        {
            return model.GetType().GetProperties()
                .ToDictionary(x => x.GetSQLColumnName(), x => x.GetValue(model));
        }
    }
}
