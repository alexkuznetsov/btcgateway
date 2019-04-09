using System;
using System.Data;

namespace BTCGatewayAPI.Infrastructure.DB.Extensions
{
    public static class DataRecordExtensions
    {
        [Obsolete("Требуется оптимизация")]
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
