using SAE.Framework.AspNetCore.Plugin;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.Framework.Plugin.FrontEnd
{
    public class Startup: WebPlugin
    {
        public override void PluginConfigureServices(IServiceCollection services)
        {
            
        }

        public override void PluginConfigure(IApplicationBuilder app)
        {

        }
    }
}
