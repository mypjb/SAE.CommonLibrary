using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Scope;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Ĭ������ע����
    /// </summary>
    public static class DefaultScopeServiceCollectionExtensions
    {
        /// <summary>
        /// ���Ĭ������ʵ��
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultScope(this IServiceCollection services)
        {
            services.AddDefaultLogger();
            services.TryAddSingleton<IScopeFactory, DefaultScopeFactory>();
            return services;
        }
    }
}