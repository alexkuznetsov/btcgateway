namespace BTCGatewayAPI.Infrastructure.Container
{
    public static class ObjectRegistryServiceExtensions
    {
        public static TService GetService<TService>(this ObjectRegistry registry)
        {
            return (TService)registry.GetService(typeof(TService));
        }
    }
}