using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BTCGatewayAPI.Infrastructure.DB
{
    public abstract partial class DbReaderMappingSpec<TModel> where TModel : class, new()
    {
        private static readonly object _lockObject = new object();
        private readonly Dictionary<string, PropertyInfo> _propertiesCache = new Dictionary<string, PropertyInfo>();
        private readonly Dictionary<string, string> _propertyMappings = new Dictionary<string, string>();

        private static bool _mappingsInitialized = false;

        private IReadOnlyDictionary<string, PropertyInfo> PropertiesCache => _propertiesCache;
        private IReadOnlyDictionary<string, string> PropertyMappings => _propertyMappings;

        //public string SQLTableName { get; set; }

        private List<Func<IDataReader, TModel, (bool, string)>> Actions { get; } = new List<Func<IDataReader, TModel, (bool, string)>>();

        private TModel _cached;

        public (bool, string[]) IsInvalid(IDataReader reader)
        {
            if (!_mappingsInitialized)
            {
                InitMappings(typeof(TModel));
            }

            _cached = new TModel();

            var hasAnyFailsForRequired = Actions.Select(x => x(reader, _cached))
                .Where(x => x.Item1 == false)
                .ToArray();

            return (hasAnyFailsForRequired.Any(), hasAnyFailsForRequired.Select(x => x.Item2).ToArray());
        }

        public TModel Map(IDataReader reader)
        {
            if (_cached != null)
                return _cached;

            if (!_mappingsInitialized)
            {
                InitMappings(typeof(TModel));
            }

            _cached = new TModel();

            Actions.ForEach(a =>
            {
                a(reader, _cached);
            });

            return _cached;
        }

        protected abstract void MapProperties();

        protected void Map<TVal>(Expression<Func<TModel, TVal>> getter, Func<IDataReader, string, (bool, TVal)> valueProvider)
        {
            var propName = (getter.Body as MemberExpression).Member.Name;
            var mappedMember = PropertyMappings[propName];
            var prop = PropertiesCache[propName];
            var setter = prop.GetSetMethod();
            var withRequiredAttribute = prop.GetCustomAttribute<RequiredAttribute>() != null;
            var nullableValueType = prop.PropertyType.IsGenericType && typeof(Nullable<>).IsAssignableFrom(prop.PropertyType.GetGenericTypeDefinition());
            var required = !nullableValueType || withRequiredAttribute;

            var setFunc = new Func<IDataReader, TModel, (bool, string)>((r, o) =>
            {
                (var success, var val) = valueProvider(r, mappedMember);

                if (success)
                {
                    setter.Invoke(o, new object[] { val });
                    return (true, mappedMember);
                }

                if (!required)
                    return (true, mappedMember);

                return (success, mappedMember);
            });

            Actions.Add(setFunc);
        }

        private void InitMappings(Type type)
        {
            lock (_lockObject)
            {
                if (!_mappingsInitialized)
                {
                    var attr = type.GetAttr<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();

                    //SQLTableName = attr?.Name ?? type.Name.ToLower();

                    foreach (var x in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        _propertiesCache.Add(x.Name, x);
                    }

                    foreach (var p in PropertiesCache)
                    {
                        var propName = p.Key;
                        var accessName = p.Value.GetAccessName();
                        _propertyMappings[propName] = accessName;
                    }

                    MapProperties();

                    _mappingsInitialized = true;
                }
            }
        }
    }
}