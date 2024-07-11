using SAE.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.Framework.Caching.Memory
{
    /// <summary>
    /// 內存实现
    /// </summary>
    public class MemoryCache : IMemoryCache
    {
        private IList<string> keys { get; }
        private readonly Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="cache">内存</param>
        public MemoryCache(Microsoft.Extensions.Caching.Memory.IMemoryCache cache)
        {
            this.keys = new List<string>();
            this._cache = cache;
        }
        ///<inheritdoc/>
        public Task<bool> AddAsync<T>(CacheDescription<T> description)
        {
            if (description.Value == null) return Task.FromResult(false);

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
        ///<inheritdoc/>
        public async Task<IEnumerable<bool>> AddAsync<T>(IEnumerable<CacheDescription<T>> descriptions)
        {
            IList<bool> results = new List<bool>();
            foreach (var description in descriptions)
            {
                results.Add(await this.AddAsync(description));
            }
            return results;
        }
        ///<inheritdoc/>
        public async Task<bool> ClearAsync()
        {
            await this.DeleteAsync(this.keys);
            return true;
        }
        ///<inheritdoc/>
        public virtual Task<T> GetAsync<T>(string key)
        {
            object @object;
            this._cache.TryGetValue(key, out @object);
            return Task.FromResult(@object == null ? default(T) : (T)@object);
        }
        ///<inheritdoc/>
        public virtual async Task<IEnumerable<T>> GetAsync<T>(IEnumerable<string> keys)
        {
            IList<T> results = new List<T>();
            foreach (var key in keys)
            {
                results.Add(await this.GetAsync<T>(key));
            }
            return results;
        }
        ///<inheritdoc/>
        public Task<IEnumerable<string>> GetKeysAsync()
        {
            return Task.FromResult<IEnumerable<string>>(this.keys);
        }
        ///<inheritdoc/>
        public Task<bool> DeleteAsync(string key)
        {
            this._cache.Remove(key);
            this.keys.Remove(key);
            return Task.FromResult(true);
        }
        ///<inheritdoc/>
        public async Task<IEnumerable<bool>> DeleteAsync(IEnumerable<string> keys)
        {
            IList<bool> results = new List<bool>();
            while (this.keys.Count > 0)
            {
                results.Add(await this.DeleteAsync(this.keys[0]));
            }
            return results;
        }

        /// <inheritdoc/>
        public Task<bool> ExistAsync(string key)
        {
            return Task.FromResult(this._cache.TryGetValue(key, out object _));
        }
    }
}
