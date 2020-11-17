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
    }
}
