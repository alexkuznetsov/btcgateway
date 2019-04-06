using System;
using System.Linq;

namespace BTCGatewayAPI.Infrastructure.Linq
{
    public class ProjectorMapping
    {
        private readonly Type _fromType;
        private Type _mapping;
        private readonly Type _toType;

        public ProjectorMapping(Type from, Type toType)
        {
            _fromType = from;
            _toType = toType;

            var propertiesFrom = from.GetProperties(System.Reflection.BindingFlags.GetProperty)
                .Where(x => x.CanWrite)
                .ToDictionary(x => x.Name, x => x);

            var propertiesTo = toType.GetProperties(System.Reflection.BindingFlags.GetProperty)
                .Where(x => x.CanWrite)
                .ToDictionary(x => x.Name, x => x);



        }

        public void UsingMapping(Type mapping)
        {
            _mapping = mapping;
        }
    }
}
