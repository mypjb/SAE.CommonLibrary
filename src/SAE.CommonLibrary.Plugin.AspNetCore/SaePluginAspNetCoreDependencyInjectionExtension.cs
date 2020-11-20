using SAE.CommonLibrary.Plugin;
using SAE.CommonLibrary.Plugin.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using SAE.CommonLibrary.Logging;
using SAE.CommonLibrary.Extension;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SaePluginAspNetCoreDependencyInjectionExtension
    {

        /// <summary>
        /// 添加插件管理(web)
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddPluginManage(this IServiceCollection services)
        {
            return services.AddPluginManage(new PluginOptions());
        }

        /// <summary>
        /// 添加插件管理(web)
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddPluginManage(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddPluginManage(configuration.GetValue<PluginOptions>(PluginOptions.Option));
        }

        /// <summary>
        /// 添加插件管理(web)
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddPluginManage(this IServiceCollection services, PluginOptions options)
        {
            services.AddNlogLogger();

            if (!services.IsRegister<IPluginManage>())
            {
                IPluginManage pluginManage = new WebPluginManage(options ??new PluginOptions());

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
            var logging = builder.ApplicationServices.GetService<ILogging<WebPluginManage>>();

            var pluginManage = builder.ApplicationServices.GetService<IPluginManage>();

            foreach (WebPlugin webPlugin in pluginManage.Plugins.Where(s => s.Status).OfType<WebPlugin>())
            {
                IPlugin plugin = webPlugin;

                logging.Info($"start load '{plugin.Name}' : {plugin.ToJsonString()}");

                try
                {
                    webPlugin.PluginConfigure(builder);

                    logging.Info($"end load '{plugin.Name}'");
                }catch(Exception ex)
                {
                    logging.Error(ex, $"load '{plugin.Name}' failure");
                    throw ex;
                }
                
            }
            return builder;
        }
    }
}
