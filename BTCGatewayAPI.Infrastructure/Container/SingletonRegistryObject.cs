using System;

namespace BTCGatewayAPI.Infrastructure.Container
{
    internal class SingletonRegistryObject : RegistryObject
    {
        private readonly object _instance;

        public SingletonRegistryObject(object instance) => _instance = instance;

        public override object Instantiate(ObjectRegistry registry) => _instance;

        protected override void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                if (_instance is IDisposable d)
                {
                    d.Dispose();
                }

                Disposed = true;
            }
        }
    }
}