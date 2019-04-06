using System;
using System.Collections.Generic;
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

                command.Parameters.Add(p);
            }
        }
    }
}