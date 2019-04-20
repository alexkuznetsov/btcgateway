using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;

namespace BTCGatewayAPI.Infrastructure
{
    public sealed class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var result = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            context.Result = new ErrorMessageResult(context.Request, context.Exception);
        }
    }
}