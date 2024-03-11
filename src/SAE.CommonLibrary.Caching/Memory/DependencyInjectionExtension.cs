using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Caching.Memory;
using IDistributedCache = SAE.CommonLibrary.Caching.IDistributedCache;
using IMemoryCache = SAE.CommonLibrary.Caching.IMemoryCache;
using MemoryCache = SAE.CommonLibrary.Caching.Memory.MemoryCache;

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
        /// <param name="serviceDescriptors">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddSAEMemoryCache(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddMemoryCache();
            serviceDescriptors.TryAddSingleton<IMemoryCache,MemoryCache>();
            return serviceDescriptors;
        }
        /// <summary>
        /// 添加基于本机内存的<see cref="IDistributedCache"/>实现
        /// </summary>
        /// <param name="serviceDescriptors">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddSAEMemoryDistributedCache(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddMemoryCache();
            serviceDescriptors.TryAddSingleton<IDistributedCache, MemoryDistributedCache>();
            serviceDescriptors.AddDefaultLogger();
            return serviceDescriptors;
        }
    }
}
