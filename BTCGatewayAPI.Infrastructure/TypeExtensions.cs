using System;
using System.Linq;

namespace BTCGatewayAPI.Infrastructure
{
    public static class TypeExtensions
    {
        public static System.Reflection.ConstructorInfo GetFirstConstructor(this Type type)
        {
            return type.GetConstructors(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .FirstOrDefault();
        }
    }
}