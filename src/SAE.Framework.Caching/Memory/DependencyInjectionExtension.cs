using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.Framework;
using SAE.Framework.Caching.Memory;
using IDistributedCache = SAE.Framework.Caching.IDistributedCache;
using IMemoryCache = SAE.Framework.Caching.IMemoryCache;
using MemoryCache = SAE.Framework.Caching.Memory.MemoryCache;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 缓存依赖注入配置类
    /// </summary>
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// 添加基于本机内存的<see cref="IMemoryCache"/>实现
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <returns>服务集合</returns>
        public static ISAEFrameworkBuilder AddMemoryCache(this ISAEFrameworkBuilder builder)
        {
            var services = builder.Services;
            services.AddMemoryCache();
            services.TryAddSingleton<IMemoryCache,MemoryCache>();
            return builder;
        }
        /// <summary>
        /// 添加基于本机内存的<see cref="IDistributedCache"/>实现
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <returns>服务集合</returns>
        public static ISAEFrameworkBuilder AddMemoryDistributedCache(this ISAEFrameworkBuilder builder)
        {
            var services = builder.Services;
            services.AddMemoryCache();
            services.TryAddSingleton<IDistributedCache, MemoryDistributedCache>();
            builder.AddDefaultLogger();
            return builder;
        }
    }
}
