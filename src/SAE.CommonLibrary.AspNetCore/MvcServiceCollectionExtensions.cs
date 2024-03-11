using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary;
using SAE.CommonLibrary.AspNetCore.Filters;
using SAE.CommonLibrary.AspNetCore.Routing;
using SAE.CommonLibrary.Extension;
using Constants = SAE.CommonLibrary.AspNetCore.Constants;
using ABAC = SAE.CommonLibrary.AspNetCore.Authorization.ABAC;
using SAE.CommonLibrary.AspNetCore.Authorization.ABAC;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// MVC注册扩展程序
    /// </summary>
    public static class MvcServiceCollectionExtensions
    {

        /// <summary>
        /// 拦截错误响应，并将其重置为<see cref="ErrorOutput"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddResponseResult(this IMvcBuilder builder)
        {
            builder.AddMvcOptions(options =>
            {
                options.Filters.Add<ResponseResultFilter>(FilterScope.First);
            }).AddNewtonsoftJson();
            return builder;
        }
        /// <summary>
        /// 扫描整站路由
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRoutingScanning(this IServiceCollection services)
        {
            services.AddMvcCore()
                    .AddApiExplorer();

            services.TryAddSingleton<IPathDescriptorProvider, PathDescriptorProvider>();
            return services;
        }
        /// <summary>
        /// 添加ABAC授权上下文
        /// </summary>
        /// <param name="services"></param>
        /// <param name="policyName">策略名称</param>
        public static IServiceCollection AddABACAuthorizationWeb(this IServiceCollection services, string policyName = null)
        {
            services.AddSAEMemoryCache();

            services.AddOptions<List<ABAC.AspNetCoreAuthDescriptor>>()
                    .Bind(Constants.Authorize.ABAC.AuthDescriptors);
            
            services.AddOptions<List<SAE.CommonLibrary.Abstract.Authorization.ABAC.AuthorizationPolicy>>()
                    .Bind(Constants.Authorize.ABAC.Policies);

            if (!services.IsRegister<IAuthorizationHandler, ABAC.AuthorizationHandler>())
            {
                services.AddSingleton<IAuthorizationHandler, ABAC.AuthorizationHandler>();
            }

            if (!services.IsRegister<IHttpRuleContextAppend, ABAC.HttpRuleContextAppend>())
            {
                services.AddSingleton<IHttpRuleContextAppend, ABAC.HttpRuleContextAppend>();
            }

            if (!services.IsRegister<IHttpRuleContextAppend, ABAC.UserHttpRuleContextAppend>())
            {
                services.AddSingleton<IHttpRuleContextAppend, ABAC.UserHttpRuleContextAppend>();
            }

            services.AddABACAuthorization()
                    .AddRuleContextProvider<ABAC.HttpRuleContextProvider>()
                    .AddAuthorizeService<ABAC.AspNetCoreAuthorizeService>();

            services.PostConfigure<AuthorizationOptions>(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                    .AddRequirements(new ABAC.ABACAuthorizationRequirement())
                                    // .Combine(options.DefaultPolicy)
                                    .Build();

                if (policyName.IsNullOrWhiteSpace())
                {
                    if (options.DefaultPolicy.Requirements != null &&
                        !options.DefaultPolicy.Requirements.OfType<ABAC.ABACAuthorizationRequirement>().Any())
                    {
                        options.DefaultPolicy = policy;
                    }
                }
                else
                {
                    options.AddPolicy(policyName, policy);
                }
            });

            return services;
        }

        /// <summary>
        /// 使用默认<see cref="Constants.Route.DefaultPath"/>配置路由扫描中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRoutingScanning(this IApplicationBuilder app)
        {
            return app.UseRoutingScanning(Constants.Route.DefaultPath);
        }
        /// <summary>
        /// 使用<paramref name="pathString"/>配置路由中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="pathString">对外暴露的访问路径</param>
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
                         body = new ErrorOutput(SAE.CommonLibrary.StatusCodes.RequestInvalid);
                         context.Response.StatusCode = 400;
                     }

                     await context.Response.WriteAsync(body.ToJsonString());
                 });
             });
            return app;
        }

        /// <summary>
        /// 配置ABACWeb授权策略
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseABACAuthorizationWeb(this IApplicationBuilder app)
        {
            app.UseAuthentication()
               .UseAuthorization();

            return app;
        }

    }
}
