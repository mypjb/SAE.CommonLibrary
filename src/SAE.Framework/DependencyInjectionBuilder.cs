using Microsoft.Extensions.DependencyInjection;

namespace SAE.Framework.Abstract
{
    /// <summary>
    /// 依赖构建者抽象实现
    /// </summary>
    public abstract class DependencyInjectionBuilder
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="services">服务集合</param>
        public DependencyInjectionBuilder(IServiceCollection services)
        {
            Services = services;
        }
        /// <summary>
        /// 服务集合
        /// </summary>
        public IServiceCollection Services { get; }
    }
}
