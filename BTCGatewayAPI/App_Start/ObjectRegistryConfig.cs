using BTCGatewayAPI.Infrastructure;
using BTCGatewayAPI.Infrastructure.Container;
using System;

namespace BTCGatewayAPI
{
    public static class ObjectRegistryConfig
    {
        private static ObjectRegistry objectRegisrty;

        public static ObjectFactory Configure()
        {
            var builder = new ContainerBuilder();

            var accessor = new Func<string, string>((name) => System.Configuration.ConfigurationManager.AppSettings.Get(name));
            var csAccessor = new Func<string, GlobalConf.CSSettings>((name) =>
            {
                var cs = System.Configuration.ConfigurationManager.ConnectionStrings[name];
                return new GlobalConf.CSSettings { ConnectionString = cs?.ConnectionString, ProviderName = cs?.ProviderName };
            });

            builder.Singleton(typeof(Infrastructure.GlobalConf), (r) => new GlobalConf(accessor, csAccessor));

            builder.AddProfile(new ApiContainerProfile());
            builder.AddProfile(new Infrastructure.InfrastructureContainerProfile());
            builder.AddProfile(new Models.Mapping.MappingContainerProfile());
            builder.AddProfile(new Bitcoin.BitconContainerProfile());
            builder.AddProfile(new Services.ServicesContainerProfile());

            objectRegisrty = builder.Build();

            return new ObjectFactory(objectRegisrty);
        }

        public static void Shutdown()
        {
            objectRegisrty.CleanUpMapping();
        }
    }
}