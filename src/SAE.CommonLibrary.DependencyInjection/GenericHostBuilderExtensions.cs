using Autofac.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    public static class GenericHostBuilderExtensions
    {
        /// <summary>
        /// 使用autofac作为依赖注入容器
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseAutofacProviderFactory(this IHostBuilder builder)
        {
            return builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        }
    }
}
