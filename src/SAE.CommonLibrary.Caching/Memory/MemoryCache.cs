using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Caching.Memory
{
    public class MemoryCache : IMemoryCache
    {
        private IList<string> keys { get; }
        private readonly Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;

        public MemoryCache(Microsoft.Extensions.Caching.Memory.IMemoryCache cache)
        {
            this.keys = new List<string>();
            this._cache = cache;
        }
        public Task<bool> AddAsync(CacheDescription description)
        {
            using (var entry = this._cache.CreateEntry(description.Key))
            {
                entry.Value = description.Value;

                if (description.Limit == CacheLimit.Nothing)
                {
                    entry.AbsoluteExpiration = null;
                    entry.AbsoluteExpirationRelativeToNow = null;
                    entry.SlidingExpiration = null;
                }
                else if (description.Limit == CacheLimit.Absolute)
                {
                    entry.AbsoluteExpiration = description.GetDateTimeOffset();
                }
                else
                {
                    entry.SlidingExpiration = description.SlidingExpiration;
                }
                entry.Dispose();
            }
            if (!this.keys.Contains(description.Key))
            {
                this.keys.Add(description.Key);
            }
            return Task.FromResult(true);
        }

        public async Task<IEnumerable<bool>> AddAsync(IEnumerable<CacheDescription> descriptions)
        {
            IList<bool> resutls = new List<bool>();
            foreach (var description in descriptions)
            {
                resutls.Add(await this.AddAsync(description));
            }
            return resutls;
        }

        public async Task<bool> ClearAsync()
        {
            await this.RemoveAsync(this.keys);
            return true;
        }

        public virtual Task<object> GetAsync(string key)
        {
            object @object;
            this._cache.TryGetValue(key,out @object);
            return Task.FromResult(@object);
        }

        public virtual async Task<IEnumerable<object>> GetAsync(IEnumerable<string> keys)
        {
            IList<object> results = new List<object>();
            foreach (var key in keys)
            {
                results.Add(await this.GetAsync(key));
            }
            return results;
        }

        public Task<IEnumerable<string>> GetKeysAsync()
        {
            return Task.FromResult<IEnumerable<string>>(this.keys);
        }

        public Task<bool> RemoveAsync(string key)
        {
            this._cache.Remove(key);
            this.keys.Remove(key);
            return Task.FromResult(true);
        }

        public async Task<IEnumerable<bool>> RemoveAsync(IEnumerable<string> keys)
        {
            IList<bool> results = new List<bool>();
            foreach (var key in keys)
            {
                results.Add(await this.RemoveAsync(key));
            }
            return results;
        }
    }
}
