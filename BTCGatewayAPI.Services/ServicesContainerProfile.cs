using BTCGatewayAPI.Infrastructure.Container;
using System.Data.Common;

namespace BTCGatewayAPI.Services
{
    public class ServicesContainerProfile : ContainerProfile
    {
        public ServicesContainerProfile()
        {
            Singleton(typeof(IPasswordHasher), (r) => new PasswordHasher());

            Transient(typeof(DbConnection), (r) =>
            {
                var conf = r.GetService<Infrastructure.GlobalConf>();
                var dbProviderFactory = r.GetService<DbProviderFactory>();
                var connection = dbProviderFactory.CreateConnection();
                connection.ConnectionString = conf.ConnectionString.ConnectionString;

                return connection;
            });

            Singleton(typeof(System.Runtime.Caching.MemoryCache),
                (_) => System.Runtime.Caching.MemoryCache.Default);
        }
    }
}
