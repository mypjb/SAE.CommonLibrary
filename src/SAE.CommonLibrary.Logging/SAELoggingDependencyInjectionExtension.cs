using System.Globalization;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 日志记录器依赖注入
    /// </summary>
    public static class SAELoggingDependencyInjectionExtension
    {
        /// <summary>
        /// 添加<see cref="ILoggingFactory"/>实现
        /// </summary>
        /// <param name="serviceCollection">服务集合</param>
        /// <returns><paramref name="serviceCollection"/></returns>
        public static IServiceCollection AddLogger<TLoggingFactory>(this IServiceCollection serviceCollection) where TLoggingFactory : class, ILoggingFactory
        {
            if (serviceCollection.IsAddEmptyLoggingFactory())
            {
                serviceCollection.AddSingleton<ILoggingFactory, TLoggingFactory>();
            }
            else
            {
                serviceCollection.TryAddSingleton<ILoggingFactory, TLoggingFactory>();
            }

            serviceCollection.TryAddSingleton(typeof(ILogging<>), typeof(Logging<>));

            return serviceCollection;
        }

        /// <summary>
        /// 添加空的日志记录工厂
        /// </summary>
        /// <param name="serviceCollection">服务集合</param>
        /// <returns><paramref name="serviceCollection"/></returns>
        public static IServiceCollection AddDefaultLogger(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogger<EmptyLoggingFactory>();
            return serviceCollection;
        }
        /// <summary>
        /// 是否注册了<see cref="EmptyLoggingFactory"/>
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>true：已存在</returns>
        public static bool IsAddEmptyLoggingFactory(this IServiceCollection services)
        {
            return !services.Any(s => s.ServiceType == typeof(ILoggingFactory) &&
                                 s.ImplementationType != typeof(EmptyLoggingFactory));
        }
    }

}
