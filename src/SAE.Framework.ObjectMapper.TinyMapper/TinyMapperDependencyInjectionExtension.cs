using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.Framework;
using SAE.Framework.ObjectMapper;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="IObjectMapper"/>配置
    /// </summary>
    public static class TinyMapperDependencyInjectionExtension
    {
        /// <summary>
        /// 注册<see cref="IObjectMapper"/>实现
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <returns><seealso cref="IObjectMapperBuilder"/></returns>
        public static IObjectMapperBuilder AddTinyMapper(this ISAEFrameworkBuilder builder)
        {
            var services = builder.Services;
            services.TryAddSingleton<IObjectMapper, TinyMapper>();
            var mapperBuilder= new ObjectMapperBuilder(services);
            services.AddSingleton<IObjectMapperBuilder>(mapperBuilder);

            return mapperBuilder;
        }

        /// <summary>
        /// 添加映射配置
        /// </summary>
        /// <param name="objectMapperBuilder">构造器</param>
        /// <param name="configAction">配置委托</param>
        /// <returns><paramref name="objectMapperBuilder"/>构造器</returns>
        public static IObjectMapperBuilder AddBuilder(this IObjectMapperBuilder objectMapperBuilder,Action<IObjectMapper> configAction)
        {
            objectMapperBuilder.Add(configAction);
            return objectMapperBuilder;
        }

    }
}
