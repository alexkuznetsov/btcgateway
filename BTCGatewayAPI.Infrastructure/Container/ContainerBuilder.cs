using System.Collections.Generic;

namespace BTCGatewayAPI.Infrastructure.Container
{
    public sealed class ContainerBuilder : ContainerBuilderCore
    {
        private Queue<ContainerProfile> ContainerProfiles { get; set; } = new Queue<ContainerProfile>();

        public ObjectRegistry Build()
        {
            var registry = new ObjectRegistry();

            ExecuteActionsOnRegistry(registry);

            while (ContainerProfiles.Count > 0)
            {
                var profile = ContainerProfiles.Dequeue();
                profile.FillActions(registry);
            }

            return registry;
        }

        public void AddProfile(ContainerProfile profile)
        {
            ContainerProfiles.Enqueue(profile);
        }
    }
}