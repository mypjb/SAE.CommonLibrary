using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Caching.Memory
{
    /// <summary>
    /// 基于内存的分布式缓存实现
    /// </summary>
    public class MemoryDistributedCache : MemoryCache, IDistributedCache
    {
        private readonly ILogging<MemoryDistributedCache> _logging;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="cache">内存</param>
        /// <param name="logging">日志记录器</param>
        public MemoryDistributedCache(Microsoft.Extensions.Caching.Memory.IMemoryCache cache,
                                      ILogging<MemoryDistributedCache> logging) : base(cache)
        {
            this._logging = logging;
            this._logging.Warn("您正在使用基于内存的分布式缓存，如果您正在构建分布式应用，建议使用AddRedisCache构建基于Redis的分布式缓存");
        }

    }
}
