using System;

namespace BTCGatewayAPI.Infrastructure.Linq
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ProjectionMappingAttribute : Attribute
    {
        private readonly Type _projectionMappingType;

        public ProjectionMappingAttribute(Type projectionMappingType)
        {
            _projectionMappingType = projectionMappingType;
        }

        public Type MappingType => _projectionMappingType;
    }
}
