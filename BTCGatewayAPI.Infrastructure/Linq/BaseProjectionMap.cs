using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace BTCGatewayAPI.Infrastructure.Linq
{
    public abstract class BaseProjectionMap<TDto, TModel> : IProjectMap
    {
        public BaseProjectionMap()
        {
            Mappings = new Dictionary<PropertyInfo, PropertyInfo>();
        }

        protected void Map(Expression<Func<TDto, object>> expression, Expression<Func<TModel, object>> toExpression)
        {
            Mappings[ExtractName(expression)] = ExtractName(toExpression);
        }


        private PropertyInfo ExtractName<TExpressionModel>(Expression<Func<TExpressionModel, object>> expression)
        {
            MemberInfo res;
            if (expression.Body is MemberExpression)
            {
                res = ((MemberExpression)expression.Body).Member;
            }
            else
            {
                var op = ((UnaryExpression)expression.Body).Operand;
                res = ((MemberExpression)op).Member;
            }

            return (PropertyInfo)res;
        }

        public IDictionary<PropertyInfo, PropertyInfo> Mappings { get; }
    }

    public interface IProjectMap
    {
        IDictionary<PropertyInfo, PropertyInfo> Mappings { get; }
    }
}
