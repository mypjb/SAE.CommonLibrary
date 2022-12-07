using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary;
using SAE.CommonLibrary.AspNetCore;
using SAE.CommonLibrary.AspNetCore.Authorization;
using SAE.CommonLibrary.AspNetCore.Filters;
using SAE.CommonLibrary.AspNetCore.Routing;
using SAE.CommonLibrary.Extension;
using Constants = SAE.CommonLibrary.AspNetCore.Constants;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="IBitmapAuthorization"/>授权构造器
    /// </summary>
    public class BitmapAuthorizationBuilder
    {
        /// <summary>
        /// 依赖注册服务
        /// </summary>
        internal readonly IServiceCollection Services;
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="services"></param>
        internal BitmapAuthorizationBuilder(IServiceCollection services)
        {
            this.Services = services;
        }
    }
    /// <summary>
    /// MVC注册扩展程序
    /// </summary>
    public static class MvcServiceCollectionExtensions
    {

        /// <summary>
        /// 添加Cors中间件依赖
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddSAECors(this IServiceCollection services, Action<CorsOptions> action)
        {
            services.AddOptions<CorsOptions>(CorsOptions.Options).Configure(action);
            services.AddDefaultLogger();
            return services;
        }

        /// <summary>
        /// 添加Cors中间件依赖
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSAECors(this IServiceCollection services)
        {
            return services.AddSAECors(_ => { });
        }

        /// <summary>
        /// 启用Cors中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSAECors(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorsMiddleware>();
            return app;
        }

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
        /// 添加基于位图的授权策略
        /// </summary>
        /// <param name="services"></param>
        /// <param name="policyName">若<paramref name="policyName"/>为空，则注册为默认的授权策略</param>
        /// <returns></returns>
        public static BitmapAuthorizationBuilder AddBitmapAuthorization(this IServiceCollection services, string policyName = null)
        {
            services.AddDefaultLogger()
                    .AddHttpContextAccessor()
                    .AddRoutingScanning()
                    .AddDefaultScope()
                    .AddSAEMemoryDistributedCache();

            if (!services.IsRegister<IAuthorizationHandler, BitmapAuthorizationHandler>())
            {
                services.AddSingleton<IAuthorizationHandler, BitmapAuthorizationHandler>();
            }

            services.TryAddSingleton<IBitmapAuthorization, BitmapAuthorization>();
            services.TryAddSingleton<IBitmapEndpointStorage, BitmapEndpointStorage>();
            services.AddOptions<List<BitmapAuthorizationDescriptor>>()
                    .Bind(BitmapAuthorizationDescriptor.Option);

            services.PostConfigure<AuthorizationOptions>(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                    .AddRequirements(new BitmapAuthorizationRequirement())
                                    // .Combine(options.DefaultPolicy)
                                    .Build();

                if (policyName.IsNullOrWhiteSpace())
                {
                    if (options.DefaultPolicy.Requirements != null &&
                        !options.DefaultPolicy.Requirements.OfType<BitmapAuthorizationRequirement>().Any())
                    {
                        options.DefaultPolicy = policy;
                    }
                }
                else
                {
                    options.AddPolicy(policyName, policy);
                }
            });

            return new BitmapAuthorizationBuilder(services);
        }
        /// <summary>
        /// 添加本地位图终端配置
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static BitmapAuthorizationBuilder AddLocalBitmapEndpointProvider(this BitmapAuthorizationBuilder builder)
        {
            builder.Services.TryAddSingleton<IBitmapEndpointProvider, LocalBitmapEndpointProvider>();
            return builder;
        }

        /// <summary>
        /// 添加远程位图终端配置
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static BitmapAuthorizationBuilder AddRemoteBitmapEndpointProvider(this BitmapAuthorizationBuilder builder)
        {
            builder.Services.AddOptions<RemoteBitmapEndpointOptions>()
                            .Bind(RemoteBitmapEndpointOptions.Option);

            builder.Services.TryAddSingleton<IBitmapEndpointProvider, RemoteBitmapEndpointProvider>();
            return builder;
        }

        /// <summary>
        /// 使用默认
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static BitmapAuthorizationBuilder AddConfigurationProvider(this BitmapAuthorizationBuilder builder)
        {
            builder.Services.AddOptions<ConfigurationEndpointOptions>()
                            .Bind(ConfigurationEndpointOptions.Option);

            builder.Services.TryAddSingleton<IBitmapEndpointProvider, ConfigurationBitmapEndpointProvider>();
            return builder;
        }

        /// <summary>
        /// 配置基于位图的授权策略
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseBitmapAuthorization(this IApplicationBuilder app)
        {
            app.UseAuthentication()
               .UseAuthorization();

            return app;
        }

    }
}
