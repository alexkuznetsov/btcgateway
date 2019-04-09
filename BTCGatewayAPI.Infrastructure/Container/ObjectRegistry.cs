using System;
using System.Collections.Generic;

namespace BTCGatewayAPI.Infrastructure.Container
{
    public sealed class ObjectRegistry : IDisposable
    {
        private bool disposed;

        private readonly Dictionary<Type, RegistryObject> _mapping = new Dictionary<Type, RegistryObject>();

        #region IDisposable

        ~ObjectRegistry()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                CleanUpMapping();
                disposed = true;
            }
        }

        #endregion

        public void Transient(Type type)
        {
            _mapping[type] = new TransientRegistryObject(type);
        }

        public void Transient(Type type, Func<ObjectRegistry, object> factory)
        {
            _mapping[type] = new TransientRegistryObject(factory);
        }

        public void Singleton(Type type, Func<ObjectRegistry, object> factory)
        {
            _mapping[type] = new SingletonRegistryObject(factory(this));
        }

        public object GetService(Type service)
        {
            if (_mapping.TryGetValue(service, out var buildFunc))
            {
                return buildFunc.Instantiate(this);
            }

            return this.CreateInstance(service);
        }

        public void CleanUpMapping()
        {
            foreach (var m in _mapping)
            {
                m.Value.Dispose();
            }

            _mapping.Clear();
        }
    }
}