using System;

namespace BTCGatewayAPI.Infrastructure.Container
{
    internal static class ObjectRegistryExtensions
    {
        public static object CreateInstance(this ObjectRegistry registry, Type type)
        {
            var ctor = type.GetFirstConstructor();
            var ctorArgsArray = ctor.GetParameters();
            var args = new object[ctorArgsArray.Length];

            for (uint i = 0; i < ctorArgsArray.Length; i++)
            {
                args[i] = registry.GetService(ctorArgsArray[i].ParameterType);
            }

            return OA.GetActivator(type).Invoke(args);
        }
    }
}