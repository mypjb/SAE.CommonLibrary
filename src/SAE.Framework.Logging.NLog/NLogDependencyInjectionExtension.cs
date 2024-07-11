using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.Framework;
using SAE.Framework.Logging;
using SAE.Framework.Logging.Nlog;

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
        /// <param name="builder">服务集合</param>
        /// <returns><paramref name="builder"/></returns>
        public static ISAEFrameworkBuilder AddNlogLogger(this ISAEFrameworkBuilder builder)
        {
            var services = builder.Services;
            if (services.IsAddEmptyLoggingFactory())
            {
                services.AddOptions<LoggingOptions>()
                        .Bind(LoggingOptions.Option);
                builder.AddLogger<LoggingFactory>();
            }
            return builder;
        }
    }

}
