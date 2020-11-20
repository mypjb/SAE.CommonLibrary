using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SAE.CommonLibrary.Plugin.AspNetCore
{
    public abstract class WebPlugin : IPlugin
    {
        string IPlugin.Name { get; set; }
        string IPlugin.Description { get; set; }
        string IPlugin.Version { get; set; }
        string IPlugin.Path { get; set; }
        bool IPlugin.Status { get; set; }
        int IPlugin.Order { get; set; }

        public abstract void PluginConfigureServices(IServiceCollection services);

        public abstract void PluginConfigure(IApplicationBuilder app);

        public override string ToString()
        {
            IPlugin plugin = this;

            return $"{{\r\n\t{nameof(plugin.Name)}:{plugin.Name},\r\n\t{nameof(plugin.Version)}:{plugin.Version},\r\n\t{nameof(plugin.Status)}:{plugin.Status},\r\n\t{nameof(plugin.Order)}:{plugin.Order},\r\n\t{nameof(plugin.Description)}:{plugin.Description}}}";
        }
    }
}
