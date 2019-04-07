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
            Logger.Trace($"Request: {request}");

            if (request.Content != null)
            {
                var cnt = await request.Content.ReadAsStringAsync();
                Logger.Trace(cnt);
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            Logger.Trace($"Response: {response}");

            if (response.Content != null)
            {
                var cnt = await response.Content.ReadAsStringAsync();
                Logger.Trace(cnt);
            }

            return response;
        }
    }
}
