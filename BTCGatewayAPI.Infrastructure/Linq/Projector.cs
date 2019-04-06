using System;
using System.Collections.Generic;

namespace BTCGatewayAPI.Infrastructure.Linq
{
    public class Projector
    {
        private static readonly Dictionary<string, ProjectorMapping> _mappingCache =
            new Dictionary<string, ProjectorMapping>();

        private static readonly object _lockerObj = new object();

        public static Projector CreateMapping<TModel, TDto>()
        {
            return new Projector(typeof(TModel), typeof(TDto));
        }

        private readonly string _key;

        public Projector(Type modelType, Type dtoType)
        {
            _key = $"{modelType.FullName}-{dtoType.FullName}";

            lock (_lockerObj)
            {
                _mappingCache[_key] = new ProjectorMapping(modelType, dtoType);
            }
        }

        public Projector Using<TMappingSource>()
        {
            lock (_lockerObj)
            {
                _mappingCache[_key].UsingMapping(typeof(TMappingSource));
            }

            return this;
        }
    }
}
