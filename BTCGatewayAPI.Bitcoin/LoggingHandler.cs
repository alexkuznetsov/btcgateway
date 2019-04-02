using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BTCGatewayAPI.Bitcoin
{
    public class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            System.Diagnostics.Trace.TraceInformation($"Request: {request}");

            if (request.Content != null)
            {
                var cnt = await request.Content.ReadAsStringAsync();
                System.Diagnostics.Trace.TraceInformation(cnt);
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            System.Diagnostics.Trace.TraceInformation($"Response: {response}");

            if (response.Content != null)
            {
                var cnt = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Trace.TraceInformation(cnt);
            }

            return response;
        }
    }
}
