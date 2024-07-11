using System;
using System.Collections.Generic;
using SAE.Framework;
using SAE.Framework.Caching;
using SAE.Framework.Caching.Redis;
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
        /// <param name="builder">服务集合</param>
        /// <returns>服务集合</returns>
        public static ISAEFrameworkBuilder AddRedisCache(this ISAEFrameworkBuilder builder)
        {
            var services = builder.Services;
            services.AddOptions<RedisOptions>()
                              .Bind(RedisOptions.Option);
            services.AddSAEFramework()
                    .AddDefaultLogger();
            services.AddSingleton<IDistributedCache, RedisDistributedCache>();

            return builder;
        }

    }
}
