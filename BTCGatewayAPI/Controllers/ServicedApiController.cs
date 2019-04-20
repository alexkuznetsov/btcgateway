using System;
using System.Web.Http;

namespace BTCGatewayAPI.Controllers
{
    [Authorize]
    public abstract class ServicedApiController<TService> : ApiController
        where TService : IDisposable
    {
        protected TService Service { get; }

        protected ServicedApiController(TService service)
        {
            Service = service;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                Service.Dispose();
            }
        }
    }
}
