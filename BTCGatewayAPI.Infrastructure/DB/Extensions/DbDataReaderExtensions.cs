using System;

namespace BTCGatewayAPI.Infrastructure.DB.Extensions
{
    public static class DbDataReaderExtensions
    {
        public static bool ExtractDecimal(this System.Data.IDataReader reader, string column, out decimal value)
        {
            value = default(decimal);

            if (reader.ExtractValue(column, out var temp))
            {
                if (temp is decimal val)
                {
                    value = val;
                    return true;
                }
                return decimal.TryParse(Convert.ToString(temp), out value);
            }

            return false;
        }

        public static bool ExtractInt(this System.Data.IDataReader reader, string column, out int value)
        {
            value = default(int);

            if (reader.ExtractValue(column, out var temp))
            {
                if (temp is int val)
                {
                    value = val;
                    return true;
                }

                return int.TryParse(Convert.ToString(temp), out value);
            }

            return false;
        }

        public static bool ExtractString(this System.Data.IDataReader reader, string column, out string value)
        {
            value = default(string);

            if (!reader.HasColumn(column) || reader[column] == DBNull.Value)
                return false;

            value = Convert.ToString(reader[column]);

            return !string.IsNullOrEmpty(value);
        }

        public static bool ExtractValue(this System.Data.IDataReader reader, string column, out object value)
        {
            value = null;

            if (!reader.HasColumn(column) || reader[column] == DBNull.Value)
                return false;

            return (value = reader[column]) != null;
        }

        public static bool ExtractDateTime(this System.Data.IDataReader reader, string column, out DateTime value)
        {
            value = default(DateTime);

            if (reader.ExtractValue(column, out var temp))
            {
                if (temp is DateTime dt)
                {
                    value = dt;
                    return true;
                }

                return DateTime.TryParse(Convert.ToString(temp), System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out value);
            }

            return false;
        }
    }
}
