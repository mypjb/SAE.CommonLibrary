using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Logging;
using SAE.CommonLibrary.Logging.Nlog;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 日志注册扩展
    /// </summary>
    public static class NLogDependencyInjectionExtension
    {
        /// <summary>
        /// 添加Log默认实现
        /// </summary>
        /// <param name="serviceCollection">服务集合</param>
        /// <returns><paramref name="serviceCollection"/></returns>
        public static IServiceCollection AddNlogLogger(this IServiceCollection serviceCollection)
        {
            if (serviceCollection.IsAddEmptyLoggingFactory())
            {
                serviceCollection.AddOptions<LoggingOptions>()
                                 .Bind(LoggingOptions.Option);
                serviceCollection.AddLogger<LoggingFactory>();
            }
            return serviceCollection;
        }
    }

}
