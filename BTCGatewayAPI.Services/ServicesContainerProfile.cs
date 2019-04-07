using BTCGatewayAPI.Bitcoin;
using BTCGatewayAPI.Infrastructure.Container;

namespace BTCGatewayAPI.Services
{
    public class ServicesContainerProfile : Infrastructure.Container.ContainerProfile
    {
        public ServicesContainerProfile()
        {
            Singleton(typeof(BitcoinClientFactory), (r) =>
            {
                var service = (LoggingHandler)r.GetService(typeof(LoggingHandler));
                var conf = r.GetService<Infrastructure.GlobalConf>();
                return new BitcoinClientFactory(conf, service);
            });
        }
    }
}
