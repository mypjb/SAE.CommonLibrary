using SAE.Framework;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入扩展
    /// </summary>
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// 添加SAE注入
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>SAE注入构建接口</returns>
        public static ISAEFrameworkBuilder AddSAEFramework(this IServiceCollection services)
        {
            return new DefaultSAEFrameworkBuilder(services);
        }
    }
}
