using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Logging;
using SAE.CommonLibrary.Logging.Nlog;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static class NLogDependencyInjectionExtension
    {
        /// <summary>
        /// 添加Log默认实现
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddNlogLogger(this IServiceCollection serviceCollection)
        {
            if (!serviceCollection.IsRegister<ILoggingFactory>())
            {
                serviceCollection.AddOptions<LoggingOptions>()
                                 .Bind(LoggingOptions.Option);
                serviceCollection.TryAddSingleton<ILoggingFactory, LoggingFactory>();
                serviceCollection.TryAddSingleton(typeof(ILogging<>), typeof(Logging<>));
            }
            return serviceCollection;
        }
    }

}
