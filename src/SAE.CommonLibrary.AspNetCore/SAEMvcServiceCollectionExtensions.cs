using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using SAE.CommonLibrary.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary;
using Constant = SAE.CommonLibrary.AspNetCore.Constant;
using SAE.CommonLibrary.AspNetCore.Filters;
using SAE.CommonLibrary.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using SAE.CommonLibrary.AspNetCore.Authorization;



namespace Microsoft.Extensions.DependencyInjection
{
    public static class SAEMvcServiceCollectionExtensions
    {
        public static IMvcBuilder AddResponseResult(this IMvcBuilder builder)
        {
            builder.AddMvcOptions(options =>
            {
                options.Filters.Add<ResponseResultAttribute>(FilterScope.First);
            });
            return builder;
        }

        public static IServiceCollection AddRoutingScanning(this IServiceCollection services)
        {
            services.AddMvcCore()
                    .AddApiExplorer();
            services.TryAddSingleton<IPathDescriptorProvider, PathDescriptorProvider>();
            return services;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRoutingScanning(this IApplicationBuilder app)
        {
            return app.UseRoutingScanning(Constant.DefaultRoutesPath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="pathString"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRoutingScanning(this IApplicationBuilder app, PathString pathString)
        {
            app.Map(pathString, build =>
             {
                 var provider = build.ApplicationServices.GetService<IPathDescriptorProvider>();
                 build.Run(async context =>
                 {
                     object body;
                     if (context.Request.Method.Equals(HttpMethods.Get, StringComparison.OrdinalIgnoreCase))
                     {
                         var paths = provider.GetDescriptors();
                         body = paths;
                         context.Response.ContentType = "application/json";
                     }
                     else
                     {
                         body = new ResponseResult(StatusCode.RequestInvalid);
                     }

                     await context.Response.WriteAsync(body.ToJsonString());
                 });
             });
            return app;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public static IServiceCollection AddBitmapAuthorization(this IServiceCollection services, string policyName = null)
        {
            services.AddSingleton<IAuthorizationHandler, BitmapAuthorizationHandler>();
            services.TryAddSingleton<IBitmapAuthorization, BitmapAuthorization>();
            services.AddAuthorization(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .AddRequirements(new BitmapAuthorizationRequirement())
                                 .Build();
                if (!policyName.IsNullOrWhiteSpace())
                {
                    options.AddPolicy(policyName, policy);
                }
                else
                {
                    options.DefaultPolicy = policy;
                }
            });
            return services;
        }

    }
}
