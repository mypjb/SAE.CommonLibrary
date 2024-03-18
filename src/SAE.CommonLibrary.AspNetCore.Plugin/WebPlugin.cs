using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Plugin;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SAE.CommonLibrary.AspNetCore.Plugin
{
    /// <summary>
    /// web插件
    /// </summary>
    public abstract class WebPlugin : IPlugin
    {
        ///<inheritdoc/>
        string IPlugin.Name { get; set; }
        ///<inheritdoc/>
        string IPlugin.Description { get; set; }
        ///<inheritdoc/>
        string IPlugin.Version { get; set; }
        ///<inheritdoc/>
        string IPlugin.Path { get; set; }
        ///<inheritdoc/>
        bool IPlugin.Status { get; set; }
        ///<inheritdoc/>
        int IPlugin.Order { get; set; }
        /// <summary>
        /// 配置依赖
        /// </summary>
        /// <param name="services">服务集合</param>
        public abstract void PluginConfigureServices(IServiceCollection services);
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="app">构造器</param>
        public abstract void PluginConfigure(IApplicationBuilder app);
        ///<inheritdoc/>
        public override string ToString()
        {
            IPlugin plugin = this;

            return $"{{\r\n\t{nameof(plugin.Name)}:{plugin.Name},\r\n\t{nameof(plugin.Version)}:{plugin.Version},\r\n\t{nameof(plugin.Status)}:{plugin.Status},\r\n\t{nameof(plugin.Order)}:{plugin.Order},\r\n\t{nameof(plugin.Description)}:{plugin.Description}}}";
        }
    }
}
