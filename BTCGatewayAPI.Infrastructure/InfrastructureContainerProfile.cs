using BTCGatewayAPI.Infrastructure.DB;
using System.Configuration;
using System.Data.Common;

namespace BTCGatewayAPI.Infrastructure
{
    public class InfrastructureContainerProfile : Container.ContainerProfile
    {
        public InfrastructureContainerProfile(string defaultSQLCS)
        {
            Singleton(typeof(DbProviderFactory), (r) =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings[defaultSQLCS];
                var providerName = string.IsNullOrEmpty(connectionString.ProviderName) ? DBContext.DefaultProviderName : connectionString.ProviderName;
                return DbProviderFactories.GetFactory(providerName);
            });

            Transient(typeof(DBContext), (r) =>
            {
                var mappingRegistry = r.GetService(typeof(MapSpecRegistry)) as MapSpecRegistry;
                var connectionString = ConfigurationManager.ConnectionStrings[defaultSQLCS];
                var factory = (DbProviderFactory)r.GetService(typeof(DbProviderFactory));

                return new DBContext(mappingRegistry, factory, connectionString.ConnectionString);
            });
        }
    }
}
