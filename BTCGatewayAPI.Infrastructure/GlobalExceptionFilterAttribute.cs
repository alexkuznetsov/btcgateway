using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace BTCGatewayAPI.Infrastructure
{
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }
            //TODO Добавить разбор
            else
            {
#if DEBUG
                context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, new { status = false, error = context.Exception.ToString() });
#else
                context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, new { status = false, error = "Something went wrong. Try again later" });
#endif
            }
        }
    }
}