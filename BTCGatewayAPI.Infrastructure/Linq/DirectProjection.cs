using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BTCGatewayAPI.Infrastructure.Linq
{
    public class DirectProjection<TSource> : BaseProjector<TSource>
    {
        public DirectProjection(IQueryable<TSource> source) : base(source)
        {
        }

        public IQueryable<TDest> Map<TDest>()
        {
            var queryExpression = GetCachedExpression<TDest>() ?? BuildExpression<TDest>();

            return Source.Select(queryExpression);
        }

        protected override MemberAssignment BuildBinding(Expression parameterExpression, MemberInfo destinationProperty, IEnumerable<PropertyInfo> sourceProperties)
        {
            var sourceProperty = sourceProperties.FirstOrDefault(src => src.Name == destinationProperty.Name);

            if (sourceProperty != null)
            {
                return Expression.Bind(destinationProperty, Expression.Property(parameterExpression, sourceProperty));
            }

            return BuildExpressionCore(sourceProperty, parameterExpression, destinationProperty, sourceProperties);
        }

        protected override string GetCacheKey<TDest>()
        {
            return string.Concat(typeof(TSource).FullName, typeof(TDest).FullName);
        }
    }
}
