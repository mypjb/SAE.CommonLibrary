using SAE.CommonLibrary.Plugin.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Plugin.Constant;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
