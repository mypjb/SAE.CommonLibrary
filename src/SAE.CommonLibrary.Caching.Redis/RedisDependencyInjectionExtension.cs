using SAE.CommonLibrary.Caching;
using SAE.CommonLibrary.Caching.Redis;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RedisDependencyInjectionExtension
    {
        /// <summary>
        /// 添加默认配置项
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCache(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddOptions<RedisOptions>()
                              .Bind(RedisOptions.Option);
            serviceDescriptors.AddNlogLogger();
            serviceDescriptors.AddSingleton<IDistributedCache, RedisDistributedCache>();
            
            return serviceDescriptors;
        }
       
    }
}
