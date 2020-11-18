using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.ObjectMapper;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TinyMapperDependencyInjectionExtension
    {
        /// <summary>
        /// register tiny implement 
        /// </summary>
        /// <param name="services"></param>
        /// <returns><seealso cref="IObjectMapper"/> build class <seealso cref="IObjectMapperBuilder"/></returns>
        public static IObjectMapperBuilder AddTinyMapper(this IServiceCollection services)
        {
            services.TryAddSingleton<IObjectMapper, TinyMapper>();
            var mapperBuilder= new ObjectMapperBuilder(services);
            services.AddSingleton<IObjectMapperBuilder>(mapperBuilder);
            return mapperBuilder;
        }

        /// <summary>
        /// Add configuration
        /// </summary>
        /// <param name="objectMapperBuilder"></param>
        /// <param name="configAction">configuratioin action</param>
        /// <returns></returns>
        public static IObjectMapperBuilder AddBuilder(this IObjectMapperBuilder objectMapperBuilder,Action<IObjectMapper> configAction)
        {
            objectMapperBuilder.Add(configAction);
            return objectMapperBuilder;
        }

    }
}
