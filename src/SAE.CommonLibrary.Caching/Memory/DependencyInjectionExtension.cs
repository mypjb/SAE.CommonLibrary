using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Caching.Memory;
using IDistributedCache = SAE.CommonLibrary.Caching.IDistributedCache;
using IMemoryCache = SAE.CommonLibrary.Caching.IMemoryCache;
using MemoryCache = SAE.CommonLibrary.Caching.Memory.MemoryCache;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddSaeMemoryCache(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddMemoryCache();
            serviceDescriptors.TryAddSingleton<IMemoryCache,MemoryCache>();
            return serviceDescriptors;
        }

        public static IServiceCollection AddSaeMemoryDistributedCache(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddSaeMemoryDistributedCache();
            serviceDescriptors.TryAddSingleton<IDistributedCache, DistributedCache>();
            return serviceDescriptors;
        }
    }
}
