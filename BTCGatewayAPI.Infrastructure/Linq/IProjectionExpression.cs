using System.Linq;

namespace BTCGatewayAPI.Infrastructure.Linq
{
    public interface IProjectionExpression
    {
        IQueryable<TResult> To<TResult>();
    }
}
