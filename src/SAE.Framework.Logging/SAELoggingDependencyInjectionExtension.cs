using System.Globalization;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.Framework;
using SAE.Framework.Logging;

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
        /// <param name="builder">服务集合</param>
        /// <returns><paramref name="builder"/></returns>
        public static ISAEFrameworkBuilder AddLogger<TLoggingFactory>(this ISAEFrameworkBuilder builder) where TLoggingFactory : class, ILoggingFactory
        {
            var services = builder.Services;
            if (services.IsAddEmptyLoggingFactory())
            {
                services.AddSingleton<ILoggingFactory, TLoggingFactory>();
            }
            else
            {
                services.TryAddSingleton<ILoggingFactory, TLoggingFactory>();
            }

            services.TryAddSingleton(typeof(ILogging<>), typeof(Logging<>));

            return builder;
        }

        /// <summary>
        /// 添加空的日志记录工厂
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <returns><paramref name="builder"/></returns>
        public static ISAEFrameworkBuilder AddDefaultLogger(this ISAEFrameworkBuilder builder)
        {
            builder.AddLogger<EmptyLoggingFactory>();
            return builder;
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
