using System;
using System.Collections.Generic;
using SAE.CommonLibrary.Caching;
using SAE.CommonLibrary.Caching.Redis;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// redis缓存接口配置类
    /// </summary>
    public static class RedisDependencyInjectionExtension
    {
        /// <summary>
        /// 添加默认配置项
        /// </summary>
        /// <param name="serviceDescriptors">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddRedisCache(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddOptions<RedisOptions>()
                              .Bind(RedisOptions.Option);
            serviceDescriptors.AddDefaultLogger();
            serviceDescriptors.AddSingleton<IDistributedCache, RedisDistributedCache>();

            return serviceDescriptors;
        }

    }
}
