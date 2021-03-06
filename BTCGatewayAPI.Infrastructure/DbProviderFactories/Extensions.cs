﻿using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace BTCGatewayAPI.Common.DbProviderFactories
{
    public static class Extensions
    {
        /// <summary>
        /// Register DB Connection factory and connection factory
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDbConnection(this IServiceCollection services)
        {
            services.AddSingleton((r) =>
            {
                var conf = r.GetRequiredService<GlobalConf>();

                return DbProviderFactoriesFake.GetFactory(conf.ConnectionString.ProviderName);
            });

            services.AddTransient((r) =>
            {
                var factory = r.GetRequiredService<DbProviderFactory>();
                var connection = factory.CreateConnection();
                var conf = r.GetRequiredService<GlobalConf>();

                connection.ConnectionString = conf.ConnectionString.ConnectionString;

                return connection;
            });

            return services;
        }
    }
}
