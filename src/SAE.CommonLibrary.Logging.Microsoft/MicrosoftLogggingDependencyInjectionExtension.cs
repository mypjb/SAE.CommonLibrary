using Microsoft.Extensions.Logging;
using SAE.CommonLibrary.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static class MicrosoftLogggingDependencyInjectionExtension
    {
        /// <summary>
        /// 添加对<seealso cref=" Microsoft.Extensions.Logging"/>的代理实现
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddMicrosoftLogging(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ILoggerProvider, MicrosoftProxyLoggingProvider>();
            serviceCollection.AddDefaultLogger();
            return serviceCollection;
        }
    }

}
