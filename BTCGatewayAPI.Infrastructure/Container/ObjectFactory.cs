using System;

namespace BTCGatewayAPI.Infrastructure.Container
{
    public sealed class ObjectFactory
    {
        private readonly ObjectRegistry registry;

        public ObjectFactory(ObjectRegistry registry) => this.registry = registry;

        public object Create(Type type) => registry.GetService(type);

        public TService Create<TService>() => (TService)Create(typeof(TService));
    }
}