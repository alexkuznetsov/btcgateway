using System;

namespace BTCGatewayAPI.Infrastructure.Container
{
    internal class SingletonRegistryObject : RegistryObject
    {
        private readonly object instance;

        public SingletonRegistryObject(object instance)
        {
            this.instance = instance;
        }

        public override object Instantiate(ObjectRegistry registry)
        {
            return instance;
        }

        protected override void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                if (instance is IDisposable d)
                {
                    d.Dispose();
                }

                Disposed = true;
            }
        }
    }
}