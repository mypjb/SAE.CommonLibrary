using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract
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
