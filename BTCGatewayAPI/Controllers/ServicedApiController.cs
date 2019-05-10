using System;

namespace BTCGatewayAPI.Controllers
{
    public abstract class ServicedApiController<TService> : DefaultApiController
    {
        protected ServicedApiController(TService service)
        {
            Service = service;
        }

        public TService Service { get; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Service is IDisposable d)
                {
                    d.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
