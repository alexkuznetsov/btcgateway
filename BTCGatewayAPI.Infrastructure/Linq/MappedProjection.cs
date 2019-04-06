using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BTCGatewayAPI.Infrastructure.Linq
{
    public class MappedProjection<TSource> : BaseProjector<TSource>
    {
        private readonly IDictionary<PropertyInfo, PropertyInfo> mappingRedirects;
        private readonly Type _mappingType;

        public MappedProjection(IQueryable<TSource> source, Type TMapType) : base(source)
        {
            this._mappingType = TMapType;
            mappingRedirects = LoadPropertyOverrides(TMapType);
        }

        public IQueryable<TDest> Map<TDest>()
        {
            var queryExpression = GetCachedExpression<TDest>() ?? BuildExpression<TDest>();

            return Source.Select(queryExpression);
        }


        protected override MemberAssignment BuildBinding(Expression parameterExpression,
            MemberInfo destinationProperty,
            IEnumerable<PropertyInfo> sourceProperties)
        {
            var sourceProperty = sourceProperties.FirstOrDefault(src => src.Name == destinationProperty.Name);

            if (sourceProperty == null && mappingRedirects.ContainsKey(destinationProperty as PropertyInfo))
            {
                sourceProperty = mappingRedirects[(destinationProperty as PropertyInfo)];
            }

            return BuildExpressionCore(sourceProperty, parameterExpression, destinationProperty, sourceProperties);
        }

        IDictionary<PropertyInfo, PropertyInfo> LoadPropertyOverrides(Type type)
        {
            var act = Infrastructure.Container.OA.GetActivator(type);
            return ((IProjectMap)act()).Mappings;
        }

        protected override string GetCacheKey<TDest>()
        {
            return string.Concat(typeof(TSource).FullName, _mappingType.FullName, typeof(TDest).FullName);
        }
    }
}
