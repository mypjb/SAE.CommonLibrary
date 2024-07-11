using Microsoft.Extensions.Logging;
using SAE.Framework;
using SAE.Framework.Logging;

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
        /// <param name="builder">服务集合</param>
        /// <returns><paramref name="builder"/></returns>
        public static ISAEFrameworkBuilder AddMicrosoftLogging(this ISAEFrameworkBuilder builder)
        {
            var services = builder.Services;
            services.AddSingleton<ILoggerProvider, MicrosoftProxyLoggingProvider>();
            builder.AddDefaultLogger();
            return builder;
        }
    }

}
