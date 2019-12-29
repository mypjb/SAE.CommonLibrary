using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SAE.CommonLibrary.ObjectMapper.Core.DataStructures;
using SAE.CommonLibrary.ObjectMapper.Core.Extensions;

namespace SAE.CommonLibrary.ObjectMapper.Mappers.Caches
{
    internal sealed class MapperCache
    {
        private readonly ConcurrentDictionary<TypePair, MapperCacheItem> _cache = new ConcurrentDictionary<TypePair, MapperCacheItem>();

        public bool IsEmpty => _cache.Count == 0;

        public List<Mapper> Mappers
        {
            get
            {
                return _cache.Values
                             .OrderBy(x => x.Id)
                             .ConvertAll(x => x.Mapper);
            }
        }

        public List<MapperCacheItem> MapperCacheItems => _cache.Values.ToList();

        public MapperCacheItem AddStub(TypePair key)
        {
            return _cache.GetOrAdd(key, k => new MapperCacheItem { Id = this.GetId() });
            
        }

        public void ReplaceStub(TypePair key, Mapper mapper)
        {
            var mapperCache= this.AddStub(key);
            mapperCache.Mapper = mapper;
        }

        public MapperCacheItem Add(TypePair key, Mapper mapper)
        {
            var mapperCache = this.AddStub(key);
            mapperCache.Mapper = mapper;
            return mapperCache;
        }

        public Option<MapperCacheItem> Get(TypePair key)
        {
            MapperCacheItem result;
            if (_cache.TryGetValue(key, out result))
            {
                return new Option<MapperCacheItem>(result);
            }
            return Option<MapperCacheItem>.Empty;
        }

        private int GetId()
        {
            return _cache.Count;
        }
    }
}
