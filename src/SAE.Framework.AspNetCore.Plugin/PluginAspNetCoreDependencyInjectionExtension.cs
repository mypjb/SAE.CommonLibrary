using SAE.Framework.Plugin;
using SAE.Framework.AspNetCore.Plugin;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using SAE.Framework.Logging;
using SAE.Framework.Extension;
using SAE.Framework;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// aspnetcore 插件配置
    /// </summary>
    public static class PluginAspNetCoreDependencyInjectionExtension
    {

        /// <summary>
        /// 添加插件管理(web)
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <returns><paramref name="builder"/></returns>
        public static ISAEFrameworkBuilder AddPluginManage(this ISAEFrameworkBuilder builder)
        {
            builder.AddPluginManage(new PluginOptions());

            return builder;
        }

        /// <summary>
        /// 添加插件管理(web)
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <param name="configuration">配置对象</param>
        /// <returns><paramref name="builder"/></returns>
        public static ISAEFrameworkBuilder AddPluginManage(this ISAEFrameworkBuilder builder, IConfiguration configuration)
        {
            builder.AddPluginManage(configuration.GetSection(PluginOptions.Option).Get<PluginOptions>());
            return builder;
        }

        /// <summary>
        /// 添加插件管理(web)
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <param name="options">插件配置</param>
        /// <returns><paramref name="builder"/></returns>
        public static ISAEFrameworkBuilder AddPluginManage(this ISAEFrameworkBuilder builder, PluginOptions options)
        {
            var services = builder.Services;

            services.AddSAEFramework()
                    .AddDefaultLogger();

            if (!services.IsRegister<IPluginManage>())
            {
                IPluginManage pluginManage = new WebPluginManage(options ?? new PluginOptions());

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

            return builder;
        }
        /// <summary>
        /// 使用插件管理
        /// </summary>
        /// <param name="builder">构建器</param>
        /// <returns><paramref name="builder"/></returns>
        public static IApplicationBuilder UsePluginManage(this IApplicationBuilder builder)
        {
            var logging = builder.ApplicationServices.GetService<ILogging<WebPluginManage>>();

            var pluginManage = builder.ApplicationServices.GetService<IPluginManage>();

            if (pluginManage.Plugins.Any())
            {
                logging.Info($"plugins count:{pluginManage.Plugins.Count()}");
                logging.Info($"plugins list:{pluginManage.Plugins.Select(s => s.Name).Aggregate((a, b) => $"{a},{b}")}");
            }
            else
            {
                logging.Warn("Not loading any plugins");
            }


            foreach (WebPlugin webPlugin in pluginManage.Plugins.Where(s => s.Status).OfType<WebPlugin>())
            {
                IPlugin plugin = webPlugin;

                logging.Info($"start plugin load '{plugin.Name}':{webPlugin}");

                try
                {
                    webPlugin.PluginConfigure(builder);

                    logging.Info($"plugin '{plugin.Name}' load  complete");
                }
                catch (Exception ex)
                {
                    logging.Error(ex, $"load '{plugin.Name}' failure");
#pragma warning disable CA2200 // 再次引发以保留堆栈详细信息
                    throw ex;
#pragma warning restore CA2200 // 再次引发以保留堆栈详细信息
                }

            }
            return builder;
        }
    }
}
