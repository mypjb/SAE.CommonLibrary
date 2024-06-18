using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.Framework;
using SAE.Framework.Scope;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// 默认区域注册类
    /// </summary>
    public static class DefaultScopeServiceCollectionExtensions
    {
        /// <summary>
        /// 添加默认区域实现
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <returns><paramref name="builder"/></returns>
        public static ISAEFrameworkBuilder AddDefaultScope(this ISAEFrameworkBuilder builder)
        {
            var services = builder.Services;
            builder.AddDefaultLogger();
            services.TryAddSingleton<IScopeFactory, DefaultScopeFactory>();
            return builder;
        }
    }
}