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
        }
    }
}
