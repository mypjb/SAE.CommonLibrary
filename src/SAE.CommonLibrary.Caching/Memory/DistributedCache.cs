using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Caching.Memory
{
    public class DistributedCache : MemoryCache, IDistributedCache
    {
        public DistributedCache(Microsoft.Extensions.Caching.Memory.IMemoryCache cache) : base(cache)
        {
        }

        public override async Task<object> GetAsync(string key)
        {
            var obj = await base.GetAsync(key);
            return obj == null ? string.Empty : obj.ToJsonString();
        }
    }
}
