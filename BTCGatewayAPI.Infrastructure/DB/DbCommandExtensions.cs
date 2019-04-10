using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace BTCGatewayAPI.Infrastructure.DB
{
    internal static class DbCommandExtensions
    {
        public static void FillParameters(this DbCommand command, params KeyValuePair<string, object>[] parameters)
        {
            foreach (var pValue in parameters)
            {
                var p = command.CreateParameter();
                p.ParameterName = pValue.Key;
                p.Value = pValue.Value;

                if (p.Value == null)
                {
                    p.Value = DBNull.Value;
                }
                else
                {
                    p.DbType = SetupParameterType(pValue.Value);
                }

                command.Parameters.Add(p);
            }
        }

        private static readonly Dictionary<Type, DbType> _typeConvert = new Dictionary<Type, DbType>()
        {
            { typeof(byte[]), DbType.Binary },
            { typeof(bool), DbType.Boolean },
            { typeof(byte), DbType.Byte },
            { typeof(DateTime), DbType.DateTime },
            { typeof(decimal), DbType.Decimal },
            { typeof(double), DbType.Double },
            { typeof(Guid), DbType.Guid },
            { typeof(Int16), DbType.Int16 },
            { typeof(Int32), DbType.Int32 },
            { typeof(Int64), DbType.Int64 },
            { typeof(object), DbType.Object },
            { typeof(SByte), DbType.SByte },
            { typeof(Single), DbType.Single },
            { typeof(string), DbType.String },
            { typeof(UInt16), DbType.UInt16 },
            { typeof(UInt32), DbType.UInt32 },
            { typeof(UInt64), DbType.UInt64 },
            { typeof(System.Xml.XmlDocument), DbType.Xml }
        };

        private static DbType SetupParameterType(object value)
        {
            if (_typeConvert.TryGetValue(value.GetType(), out var dbType))
                return dbType;

            throw new ArgumentOutOfRangeException(nameof(value));
        }
    }
}