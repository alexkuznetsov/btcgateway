using System;

namespace BTCGatewayAPI.Infrastructure.Container
{
    public sealed class ObjectFactory
    {
        private readonly ObjectRegistry _registry;

        public ObjectFactory(ObjectRegistry registry) => _registry = registry;

        public object Create(Type type) => _registry.GetService(type);

        public TService Create<TService>() => (TService)Create(typeof(TService));
    }
}