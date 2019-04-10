using BTCGatewayAPI.Bitcoin;
using BTCGatewayAPI.Infrastructure.Container;
using System.Data.Common;

namespace BTCGatewayAPI.Services
{
    public class ServicesContainerProfile : ContainerProfile
    {
        public ServicesContainerProfile()
        {
            Singleton(typeof(BitcoinClientFactory), (r) =>
            {
                var service = (LoggingHandler)r.GetService(typeof(LoggingHandler));
                var conf = r.GetService<Infrastructure.GlobalConf>();
                return new BitcoinClientFactory(conf, service);
            });

            Singleton(typeof(IPasswordHasher), (r) => new PasswordHasher());

            Transient(typeof(DbConnection), (r) =>
            {
                var conf = r.GetService<Infrastructure.GlobalConf>();
                var dbProviderFactory = r.GetService<DbProviderFactory>();
                var connection = dbProviderFactory.CreateConnection();
                connection.ConnectionString = conf.ConnectionString.ConnectionString;

                return connection;
            });
        }
    }
}
