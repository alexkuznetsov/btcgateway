using System;

namespace BTCGatewayAPI.Infrastructure.Container
{
    internal class TransientRegistryObject : RegistryObject
    {
        private readonly Func<ObjectRegistry, object> builder;

        public TransientRegistryObject(Type service) : this(new Func<ObjectRegistry, object>((r) => r.CreateInstance(service)))
        {
        }

        public TransientRegistryObject(Func<ObjectRegistry, object> func) => builder = func;

        public override object Instantiate(ObjectRegistry registry) => builder(registry);
    }
}