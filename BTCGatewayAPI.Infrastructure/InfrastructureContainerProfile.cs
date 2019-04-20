using BTCGatewayAPI.Infrastructure.Container;
using System.Data.Common;

namespace BTCGatewayAPI.Infrastructure
{
    public sealed class InfrastructureContainerProfile : ContainerProfile
    {
        public InfrastructureContainerProfile()
        {
            Singleton(typeof(DbProviderFactory), (r) =>
            {
                var conf = r.GetService<GlobalConf>();
                var providerName = string.IsNullOrEmpty(conf.ConnectionString.ProviderName) ? "System.Data.SqlClient" : conf.ConnectionString.ProviderName;
                return DbProviderFactories.GetFactory(providerName);
            });
        }
    }
}
