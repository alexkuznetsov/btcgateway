using System;
using System.Collections.Generic;

namespace BTCGatewayAPI.Infrastructure.Container
{
    public abstract class ContainerBuilderCore
    {
        protected Queue<Action<ObjectRegistry>> PendingActions { get; set; } = new Queue<Action<ObjectRegistry>>();

        public void Singleton(Type type, Func<ObjectRegistry, object> factory)
        {
            PendingActions.Enqueue(new Action<ObjectRegistry>((r) =>
            {
                r.Singleton(type, factory);
            }));
        }

        public void Transient(Type type)
        {
            PendingActions.Enqueue(new Action<ObjectRegistry>((r) =>
            {
                r.Transient(type);
            }));
        }

        public void Transient(Type type, Func<ObjectRegistry, object> factory)
        {
            PendingActions.Enqueue(new Action<ObjectRegistry>((r) =>
            {
                r.Transient(type, factory);
            }));
        }

        protected void ExecuteActionsOnRegistry(ObjectRegistry registry)
        {
            while (PendingActions.Count > 0)
            {
                var action = PendingActions.Dequeue();
                action(registry);
            }
        }
    }
}