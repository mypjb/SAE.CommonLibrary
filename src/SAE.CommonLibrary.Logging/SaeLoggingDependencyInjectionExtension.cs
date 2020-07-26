using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static class SaeLoggingDependencyInjectionExtension
    {
        /// <summary>
        /// 添加Log默认实现
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddLogger(this IServiceCollection serviceCollection,ILoggingFactory loggingFactory)
        {
            serviceCollection.TryAddSingleton(loggingFactory);
            return serviceCollection;
        }
    }

}
