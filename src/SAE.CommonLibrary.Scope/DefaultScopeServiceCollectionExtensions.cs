using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SAE.CommonLibrary.Scope
{

    /// <summary>
    /// default scope service collection extensions
    /// </summary>
    public static class DefaultScopeServiceCollectionExtensions
    {
        /// <summary>
        /// 添加默认区域配置
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultScope(this IServiceCollection services)
        {
            services.AddNlogLogger();
            services.TryAddSingleton<IScopeFactory, DefaultScopeFactory>();
            return services;
        }
    }
}