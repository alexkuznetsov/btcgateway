using System;

namespace BTCGatewayAPI.Infrastructure.Container
{
    internal abstract class RegistryObject : IDisposable
    {
        protected bool Disposed { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {

        }

        public abstract object Instantiate(ObjectRegistry registry);
    }
}