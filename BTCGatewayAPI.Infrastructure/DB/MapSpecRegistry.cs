using BTCGatewayAPI.Infrastructure.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BTCGatewayAPI.Infrastructure.DB
{
    public class MapSpecRegistry
    {
        private readonly ObjectRegistry registry;

        public MapSpecRegistry(ObjectRegistry registry, params Assembly[] assemblies)
        {
            this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
            InitMappings(assemblies);
        }

        private void InitMappings(Assembly[] assemblies)
        {
            var baseType = typeof(DbReaderMappingSpec<>);
            var typeBuilder = GetType().GetMethod(nameof(GetMappingType), BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var a in assemblies)
            {
                foreach (var t in ExtractTypes(a, baseType))
                {
                    var modelType = t.BaseType.GetGenericArguments().Single();
                    var genericMethodInfo = typeBuilder.MakeGenericMethod(new[] { modelType });
                    var call = Expression.Call(Expression.Constant(this), genericMethodInfo);
                    var type = (Expression.Lambda<Func<Type>>(call).Compile())();

                    registry.Singleton(type, (r) => r.GetService(t));
                }
            }
        }

        private Type GetMappingType<TModel>() where TModel : class, new()
        {
            return typeof(DbReaderMappingSpec<TModel>);
        }

        private IEnumerable<Type> ExtractTypes(Assembly a, Type baseType)
        {
            return a.GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => x.BaseType.IsGenericType)
                .Where(x => baseType.IsAssignableFrom(x.BaseType.GetGenericTypeDefinition()));
        }

        public DbReaderMappingSpec<TModel> GetService<TModel>()
            where TModel : class, new()
        {
            return (DbReaderMappingSpec<TModel>)registry.GetService(typeof(DbReaderMappingSpec<TModel>));
        }
    }
}