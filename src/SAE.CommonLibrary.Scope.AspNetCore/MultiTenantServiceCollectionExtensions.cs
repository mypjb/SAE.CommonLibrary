using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Scope;
using SAE.CommonLibrary.Scope.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// multi tenant service collection extensions
    /// </summary>
    public static class MultiTenantServiceCollectionExtensions
    {

        /// <summary>
        /// configuration multi tenant
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMultiTenant(this IServiceCollection services)
        {
            services.AddDefaultScope();
            services.AddOptions<MultiTenantOptions>()
                    .Bind(MultiTenantOptions.Option);
            return services;
        }
        /// <summary>
        /// use multi tenant middleware
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
