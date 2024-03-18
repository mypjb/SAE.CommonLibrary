using SAE.CommonLibrary.AspNetCore.Plugin;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.CommonLibrary.Plugin.FrontEnd
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
