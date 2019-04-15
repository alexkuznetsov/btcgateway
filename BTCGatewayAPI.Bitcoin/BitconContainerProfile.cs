using BTCGatewayAPI.Infrastructure.Container;
using System.Net.Http;

namespace BTCGatewayAPI.Bitcoin
{
    public class BitconContainerProfile : Infrastructure.Container.ContainerProfile
    {
        public BitconContainerProfile()
        {
            Singleton(typeof(LoggingHandler), (r) => new LoggingHandler(
                new HttpClientHandler(),
                r.GetService<Infrastructure.GlobalConf>()));

            Singleton(typeof(BitcoinClientFactory), (r) =>
            {
                var handler = (LoggingHandler)r.GetService(typeof(LoggingHandler));
                var conf = r.GetService<Infrastructure.GlobalConf>();
                return new BitcoinClientFactory(conf, handler);
            });
        }
    }
}
