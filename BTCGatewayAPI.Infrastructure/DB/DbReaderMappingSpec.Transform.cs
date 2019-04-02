using BTCGatewayAPI.Infrastructure.DB.Extensions;
using System;

namespace BTCGatewayAPI.Infrastructure.DB
{
    public abstract partial class DbReaderMappingSpec<TModel> where TModel : class, new()
    {
        public static class Transform
        {
            public static (bool, string) GetString(System.Data.IDataReader reader, string name)
            {
                var success = reader.ExtractString(name, out var value);

                return (success, value);
            }

            public static (bool, Uri) GetUri(System.Data.IDataReader reader, string name)
            {
                var success = reader.ExtractString(name, out var value);

                if (success)
                {
                    success = Uri.TryCreate(value, UriKind.Absolute, out var uValue);
                    return (success, uValue);
                }

                return (success, null);
            }

            public static (bool, int) GetInt(System.Data.IDataReader reader, string name)
            {
                var success = reader.ExtractInt(name, out var value);

                return (success, value);
            }

            public static (bool, bool) GetBool(System.Data.IDataReader reader, string name)
            {
                var success = reader.ExtractInt(name, out var value);

                return (success, value > 0);
            }

            public static (bool, decimal) GetDecimal(System.Data.IDataReader reader, string name)
            {
                var success = reader.ExtractDecimal(name, out var value);

                return (success, value);
            }

            public static (bool, DateTime?) GetDateTimeOrNull(System.Data.IDataReader reader, string name)
            {
                var success = reader.ExtractDateTime(name, out var value);

                return (success, success ? new DateTime?(value) : null);
            }

            public static (bool, DateTime) GetDateTime(System.Data.IDataReader reader, string name)
            {
                var success = reader.ExtractDateTime(name, out var value);

                return (success, value);
            }
        }
    }
}
