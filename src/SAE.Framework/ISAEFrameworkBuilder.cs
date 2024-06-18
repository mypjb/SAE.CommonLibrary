using Microsoft.Extensions.DependencyInjection;
using SAE.Framework.Abstract;

namespace SAE.Framework
{
    /// <summary>
    /// SAE依赖注入构建接口
    /// </summary>
    public interface ISAEFrameworkBuilder
    {
        /// <summary>
        /// 服务集合
        /// </summary>
        IServiceCollection Services { get; }
    }
    /// <summary>
    /// SAE依赖注入构建器
    /// </summary>
    public class DefaultSAEFrameworkBuilder : DependencyInjectionBuilder, ISAEFrameworkBuilder
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="services"></param>
        public DefaultSAEFrameworkBuilder(IServiceCollection services) : base(services)
        {
        }
    }
}
