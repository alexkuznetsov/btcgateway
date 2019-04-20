using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BTCGatewayAPI.Infrastructure
{
    public class ErrorMessageResult : IHttpActionResult
    {
        private HttpRequestMessage _request;
        private readonly Exception _exception;

        public ErrorMessageResult(HttpRequestMessage request, Exception exception)
        {
            _request = request;
            _exception = exception;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
#if DEBUG
            var message = _request.CreateResponse(HttpStatusCode.BadRequest, new { status = false, error = _exception.ToString() });
#else
            var message = _request.CreateResponse(HttpStatusCode.BadRequest, new { status = false, error = "Something went wrong. Try again later" });
#endif

            return Task.FromResult(message);
        }
    }
}