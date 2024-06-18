using Autofac;
using Autofac.Extensions.DependencyInjection;
using System;

namespace Microsoft.Extensions.Hosting
{
    public static class GenericHostBuilderExtensions
    {
        /// <summary>
        /// 使用autofac作为依赖注入容器
        /// </summary>
        /// <param name="builder">构建器</param>
        /// <returns><paramref name="builder"/></returns>
        public static IHostBuilder UseAutofacProviderFactory(this IHostBuilder builder)
        {
            return builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        }

        /// <summary>
        /// 使用autofac作为依赖注入容器
        /// </summary>
        /// <param name="builder">构建器</param>
        /// <param name="configurationAction">容器注册委托</param>
        /// <returns><paramref name="builder"/></returns>
        public static IHostBuilder UseAutofacProviderFactory(this IHostBuilder builder, Action<ContainerBuilder> configurationAction)
        {
            return builder.UseServiceProviderFactory(new AutofacServiceProviderFactory(configurationAction));
        }
    }
}
