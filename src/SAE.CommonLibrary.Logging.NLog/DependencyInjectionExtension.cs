using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Logging;
using SAE.CommonLibrary.Logging.Implement;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// 添加Log默认实现
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddLogger(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<ILoggingFactory, LoggingFactory>();
            serviceCollection.TryAddSingleton(typeof(ILog<>),typeof(Logging<>));
            return serviceCollection;
        }
    }

}
