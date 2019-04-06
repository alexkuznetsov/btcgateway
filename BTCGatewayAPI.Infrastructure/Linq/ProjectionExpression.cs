using System.Linq;

namespace BTCGatewayAPI.Infrastructure.Linq
{
    public class ProjectionExpression<TSource> : IProjectionExpression
    {
        private readonly IQueryable<TSource> _source;

        public ProjectionExpression(IQueryable<TSource> source)

        {
            _source = source;
        }

        public IQueryable<TDest> To<TDest>()
        {
            var attr = typeof(TDest).GetAttr<ProjectionMappingAttribute>();

            if (attr != null)
            {
                var mappingType = attr.MappingType;
                return new MappedProjection<TSource>(_source, mappingType).Map<TDest>();
            }

            return new DirectProjection<TSource>(_source).Map<TDest>();
        }
    }
}
