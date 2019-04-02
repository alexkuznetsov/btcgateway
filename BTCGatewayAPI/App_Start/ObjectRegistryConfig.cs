using BTCGatewayAPI.Infrastructure.Container;

namespace BTCGatewayAPI
{
    public static class ObjectRegistryConfig
    {
        private const string DefaultSQLCS = "DefaultSQL";
        private static ObjectRegistry objectRegisrty;

        public static ObjectFactory Configure()
        {
            var builder = new ContainerBuilder();

            builder.AddProfile(new ApiContainerProfile());
            builder.AddProfile(new Infrastructure.InfrastructureContainerProfile(DefaultSQLCS));
            builder.AddProfile(new Models.Mapping.MappingContainerProfile());

            objectRegisrty = builder.Build();

            return new ObjectFactory(objectRegisrty);
        }

        public static void Shutdown()
        {
            objectRegisrty.CleanUpMapping();
        }
    }
}