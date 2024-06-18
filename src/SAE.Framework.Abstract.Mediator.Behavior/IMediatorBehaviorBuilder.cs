using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.Abstract.Mediator.Behavior
{
    /// <summary>
    /// 中介者管道构建对象
    /// </summary>
    public interface IMediatorBehaviorBuilder
    {
        /// <summary>
        /// 服务集合
        /// </summary>
        public IServiceCollection Services { get; }
    }
    /// <summary>
    /// <see cref="IMediatorBehaviorBuilder"/>实现
    /// </summary>
    public class MediatorBehaviorBuilder : DependencyInjectionBuilder, IMediatorBehaviorBuilder
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="services">服务集合</param>
        public MediatorBehaviorBuilder(IServiceCollection services) : base(services)
        {
        }
    }
}
