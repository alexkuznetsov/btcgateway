using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BTCGatewayAPI.Infrastructure.DB.Extensions
{
    internal static class ObjectExtensions
    {
        public static string TableName(this object model)
        {
            return model.GetType().GetAttr<TableAttribute>()?.Name;
        }

        public static string GetIdField(this object model)
        {
            return model.GetType().GetProperties()
                .Where(x => x.GetAttr<KeyAttribute>() != null)
                .Select(x => x.Name)
                .SingleOrDefault();
        }

        public static Dictionary<string, object> AsDictionary(this object model)
        {
            var val = new Dictionary<string, object>();

            foreach (var p in model.GetType().GetProperties())
            {
                val.Add(p.Name, p.GetValue(model));
            }

            return val;
        }
    }
}
