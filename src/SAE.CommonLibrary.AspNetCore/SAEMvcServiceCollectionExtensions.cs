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
using Constants = SAE.CommonLibrary.AspNetCore.Constants;
using SAE.CommonLibrary.AspNetCore.Filters;
using SAE.CommonLibrary.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using SAE.CommonLibrary.AspNetCore.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public class BitmapAuthorizationBuilder
    {
        internal readonly IServiceCollection Services;

        internal BitmapAuthorizationBuilder(IServiceCollection services)
        {
            this.Services = services;
        }
    }
    public static class SAEMvcServiceCollectionExtensions
    {
        /// <summary>
        /// 拦截响应将其重置为<seealso cref="ResponseResult"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddResponseResult(this IMvcBuilder builder)
        {
            builder.AddMvcOptions(options =>
            {
                options.Filters.Add<ResponseResultAttribute>(FilterScope.First);
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
        /// 使用默认<seealso cref="Constants.DefaultRoutesPath"/>配置路由扫描中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRoutingScanning(this IApplicationBuilder app)
        {
            return app.UseRoutingScanning(Constants.DefaultRoutesPath);
        }
        /// <summary>
        /// 使用<paramref name="pathString"/>配置路由中间件
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
            services.AddNlogLogger()
                    .AddHttpContextAccessor();

            services.AddSingleton<IAuthorizationHandler, BitmapAuthorizationHandler>();
            services.TryAddSingleton<IBitmapAuthorization, BitmapAuthorization>();
            services.TryAddSingleton<IBitmapEndpointStorage, BitmapEndpointStorage>();

            services.PostConfigure<AuthorizationOptions>(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                    .AddRequirements(new BitmapAuthorizationRequirement())
                                    .Combine(options.DefaultPolicy)
                                    .Build();
               
                if (policyName.IsNullOrWhiteSpace())
                {
                    options.DefaultPolicy = policy;
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
            return builder.AddLocalBitmapEndpointProvider(provider =>
            {
                var descriptors= provider.GetService<IPathDescriptorProvider>()
                                         .GetDescriptors()
                                         .OrderBy(s => s.Path)
                                         .ThenBy(s => s.Method)
                                         .ThenBy(s => s.Name)
                                         .ToArray();

                var endpoints = new List<BitmapEndpoint>();

                for (int i = 0; i < descriptors.Length; i++)
                {
                    endpoints.Add(new BitmapEndpoint
                    {
                        Path = descriptors[i].Path,
                        Index = i
                    });
                }

                return endpoints;
            });
        }

        /// <summary>
        /// 添加本地位图终端配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="pathProvider"></param>
        /// <returns></returns>
        public static BitmapAuthorizationBuilder AddLocalBitmapEndpointProvider(
                                                 this BitmapAuthorizationBuilder builder,
                                                 Func<IServiceProvider,IEnumerable<BitmapEndpoint>> endpointProvider)
        {
            builder.Services.AddSingleton<IBitmapEndpointProvider, LocalBitmapEndpointProvider>(provider=>
            {
                return new LocalBitmapEndpointProvider(endpointProvider.Invoke(provider));
            }) ;
            return builder;
        }
        /// <summary>
        /// 添加远程位图终端配置
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static BitmapAuthorizationBuilder AddRemoteBitmapEndpointProvider(this BitmapAuthorizationBuilder builder)
        {
            builder.Services.AddSaeOptions<RemoteBitmapEndpointOptions>(Constants.OptionName);
            builder.Services.TryAddSingleton<IBitmapEndpointProvider, RemoteBitmapEndpointProvider>();
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

            var storage = app.ApplicationServices.GetService<IBitmapEndpointStorage>();

            if (storage.Count() == 0)
            {
                var provider = app.ApplicationServices.GetService<IBitmapEndpointProvider>();

                var pathDescriptorProvider = app.ApplicationServices.GetService<IPathDescriptorProvider>();

                var paths = pathDescriptorProvider.GetDescriptors();

                var endpoints = provider.FindALLAsync(paths)
                                              .GetAwaiter()
                                              .GetResult();

                storage.AddRange(endpoints);
            }
            return app;
        }

     
    }
}
