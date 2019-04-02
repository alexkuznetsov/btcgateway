using System.Data;
using System.Linq;
using System.Reflection;

namespace BTCGatewayAPI.Infrastructure.DB
{
    internal static class MemberInfoExtensions
    {
        public static TAttr GetAttr<TAttr>(this MemberInfo p) => p.GetCustomAttributes(true).Where(x => x is TAttr).Select(x => (TAttr)x).FirstOrDefault();

        public static string GetAccessName(this MemberInfo p) => p.GetAttr<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>()?.Name ?? p.Name;
    }
}