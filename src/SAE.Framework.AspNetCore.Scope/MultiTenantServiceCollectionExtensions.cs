using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.Framework.Scope;
using SAE.Framework.AspNetCore.Scope;
using SAE.Framework;
using Autofac.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// multi tenant service collection extensions
    /// </summary>
    public static class MultiTenantServiceCollectionExtensions
    {

        /// <summary>
        /// 添加多租户配置
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ISAEFrameworkBuilder AddMultiTenant(this ISAEFrameworkBuilder builder)
        {
            var services = builder.Services;
            builder.AddDefaultLogger()
                   .AddDefaultScope();
            services.AddOptions<MultiTenantOptions>()
                    .Bind(MultiTenantOptions.Option);
            services.TryAddSingleton<IMultiTenantService,DefaultMultiTenantService>();
            return builder;
        }
        /// <summary>
        /// 添加多租户中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenant(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<MultiTenantMiddleware>();
            return builder;
        }

    }
}
