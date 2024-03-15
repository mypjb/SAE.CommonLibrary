using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.CommonLibrary.AspNetCore.Plugin;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SAE.CommonLibrary.Plugin.Identity
{
    public class Startup : WebPlugin
    {
        public override void PluginConfigureServices(IServiceCollection services)
        {
            var builder = services.AddIdentityServer()
                 .AddInMemoryApiScopes(Config.ApiScopes)
                 .AddInMemoryClients(Config.Clients)
                //  .AddJwtBearerClientAuthentication()
                 .AddDeveloperSigningCredential();
        }

        public override void PluginConfigure(IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }
    }
}
