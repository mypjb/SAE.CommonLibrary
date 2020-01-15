using SAE.CommonLibrary.Caching;
using SAE.CommonLibrary.Caching.Redis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// 添加默认配置项
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCache(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddSaeOptions<RedisConfig>("redis");
            serviceDescriptors.AddNlogLogger();
            serviceDescriptors.AddSingleton<IDistributedCache, RedisDistributedCache>();
            return serviceDescriptors;
        }
       
    }
}
