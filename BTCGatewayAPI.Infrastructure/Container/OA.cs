using System;
using System.Linq.Expressions;

namespace BTCGatewayAPI.Infrastructure.Container
{
    internal static class OA
    {
        internal delegate /*T*/ object ObjectActivator/*<T>*/(params object[] args);

        internal static ObjectActivator/*<T>*/ GetActivator/*<T>*/(/*ConstructorInfo ctor*/Type type)
        {
            var ctor = type.GetFirstConstructor();
            var paramsInfo = ctor.GetParameters();

            var param = Expression.Parameter(typeof(object[]), "args");
            var argsParamArray = new Expression[paramsInfo.Length];

            for (int i = 0; i < paramsInfo.Length; i++)
            {
                var paramType = paramsInfo[i].ParameterType;
                var paramAccessorExp = Expression.ArrayIndex(param, Expression.Constant(i));

                argsParamArray[i] = Expression.Convert(paramAccessorExp, paramType);
            }

            var lambda = Expression.Lambda(typeof(ObjectActivator/*<T>*/), Expression.New(ctor, argsParamArray), param);

            return (ObjectActivator/*<T>*/)lambda.Compile();
        }
    }
}