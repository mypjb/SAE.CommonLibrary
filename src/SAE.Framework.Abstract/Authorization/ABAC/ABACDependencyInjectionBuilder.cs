using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.Framework.Abstract.Authorization.ABAC
{
    /// <summary>
    /// ABAC授权依赖注入构建类
    /// </summary>
    public class ABACDependencyInjectionBuilder : DependencyInjectionBuilder
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="services"></param>
        public ABACDependencyInjectionBuilder(IServiceCollection services) : base(services)
        {
        }
    }
}