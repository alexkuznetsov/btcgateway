using BTCGatewayAPI.Infrastructure.DB;
using BTCGatewayAPI.Infrastructure.Container;
using System.Data.Common;

namespace BTCGatewayAPI.Infrastructure
{
    public class InfrastructureContainerProfile : ContainerProfile
    {
        public InfrastructureContainerProfile()
        {
            Singleton(typeof(IQueryBuilder), (r) => new MSSQLServerQueryBuilder());

            Singleton(typeof(DbProviderFactory), (r) =>
            {
                var conf = r.GetService<GlobalConf>();
                var providerName = string.IsNullOrEmpty(conf.ConnectionString.ProviderName) ? DBContext.DefaultProviderName : conf.ConnectionString.ProviderName;
                return DbProviderFactories.GetFactory(providerName);
            });

            Transient(typeof(DBContext), (r) =>
            {
                var mappingRegistry = r.GetService<MapSpecRegistry>() ;
                var conf = r.GetService<GlobalConf>();
                var factory = r.GetService<DbProviderFactory>();
                var queryBuilder = r.GetService<IQueryBuilder>();

                return new DBContext(mappingRegistry, queryBuilder, conf, factory);
            });
        }
    }
}
