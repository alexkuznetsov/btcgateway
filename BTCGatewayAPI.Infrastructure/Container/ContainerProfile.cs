namespace BTCGatewayAPI.Infrastructure.Container
{
    public abstract class ContainerProfile : ContainerBuilderCore
    {
        public void FillActions(ObjectRegistry registry)
        {
            ExecuteActionsOnRegistry(registry);
        }
    }
}
