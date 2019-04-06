using System.Linq;

namespace BTCGatewayAPI.Infrastructure.Linq
{
    public static class IQueriableExtensions
    {
        public static IProjectionExpression Project<TSource>(
            this IQueryable<TSource> source)
        {
            return new ProjectionExpression<TSource>(source);
        }
    }
}
