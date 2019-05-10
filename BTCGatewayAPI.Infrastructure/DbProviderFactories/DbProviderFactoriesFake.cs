using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace BTCGatewayAPI.Common.DbProviderFactories
{
    public static class DbProviderFactoriesFake
    {
        public static DbProviderFactory GetFactory(string providerName)
        {
            if (providerName.ToLowerInvariant().Equals("system.data.sqlclient"))
            {
                return System.Data.SqlClient.SqlClientFactory.Instance;
            }

            throw new ArgumentOutOfRangeException(nameof(providerName));
        }
    }
}
