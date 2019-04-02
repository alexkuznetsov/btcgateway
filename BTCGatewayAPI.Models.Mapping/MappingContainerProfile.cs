using BTCGatewayAPI.Infrastructure.DB;

namespace BTCGatewayAPI.Models.Mapping
{
    public class MappingContainerProfile : Infrastructure.Container.ContainerProfile
    {
        public MappingContainerProfile()
        {
            Singleton(typeof(MapSpecRegistry), (r) => new MapSpecRegistry(r, GetType().Assembly));
        }
    }
}
