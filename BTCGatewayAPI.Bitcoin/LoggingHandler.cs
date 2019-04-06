using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Bitcoin
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly Infrastructure.GlobalConf conf;

        private static readonly Lazy<Infrastructure.Logging.ILogger> LoggerLazy = new Lazy<Infrastructure.Logging.ILogger>(Infrastructure.Logging.LoggerFactory.GetLogger);

        private static Infrastructure.Logging.ILogger Logger => LoggerLazy.Value;

        public LoggingHandler(HttpMessageHandler innerHandler, Infrastructure.GlobalConf conf)
            : base(innerHandler)
        {
            this.conf = conf;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (conf.LogRequests)
            {
                Logger.Info($"Request: {request}");

                if (request.Content != null)
                {
                    var cnt = await request.Content.ReadAsStringAsync();
                    Logger.Info(cnt);
                }
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if (conf.LogRequests)
            {
                Logger.Info($"Response: {response}");

                if (response.Content != null)
                {
                    var cnt = await response.Content.ReadAsStringAsync();
                    Logger.Info(cnt);
                }
            }

            return response;
        }
    }
}
