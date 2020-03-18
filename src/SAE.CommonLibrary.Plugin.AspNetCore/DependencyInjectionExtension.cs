﻿using SAE.CommonLibrary.Plugin;
using SAE.CommonLibrary.Plugin.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// 添加插件管理(web)
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddPluginManage(this IServiceCollection services)
        {
            if (!services.IsRegister<IPluginManage>())
            {
                //services.AddSaeOptions<PluginOptions>("plugin");

                //services.TryAddSingleton<IPluginManage, WebPluginManage>();
                IPluginManage pluginManage = new WebPluginManage(new PluginOptions { Path = "../../../../Plugins/dest" });
                //var pluginManage = services.BuildServiceProvider()
                //                           .GetService<IPluginManage>();
                services.AddSingleton(s => pluginManage);

                foreach (WebPlugin plugin in pluginManage.Plugins.Where(s => s.Status).OfType<WebPlugin>())
                {
                    services.AddMvcCore().ConfigureApplicationPartManager(manage =>
                    {
                        var controllerAssemblyPart = new AssemblyPart(plugin.GetType().Assembly);
                        manage.ApplicationParts.Add(controllerAssemblyPart);
                    });
                    plugin.PluginConfigureServices(services);
                }
                
            }

            return services;
        }
        /// <summary>
        /// 使用插件管理
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UsePluginManage(this IApplicationBuilder builder)
        {
            var pluginManage = builder.ApplicationServices.GetService<IPluginManage>();
            foreach (WebPlugin plugin in pluginManage.Plugins.Where(s => s.Status).OfType<WebPlugin>())
            {
                plugin.PluginConfigure(builder);
            }
            return builder;
        }
    }
}