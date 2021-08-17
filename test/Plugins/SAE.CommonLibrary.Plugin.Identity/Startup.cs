using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.CommonLibrary.Plugin.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SAE.CommonLibrary.Plugin.Identity
{
    public class Startup:WebPlugin
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public  void ConfigureServices(IServiceCollection services)
        {
            this.PluginConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public  void Configure(IApplicationBuilder app)
        {
            this.PluginConfigure(app);
        }

        public override void PluginConfigureServices(IServiceCollection services)
        {
            var builder = services.AddIdentityServer()
                 .AddInMemoryApiScopes(Config.ApiScopes)
                 .AddInMemoryClients(Config.Clients)
                 .AddJwtBearerClientAuthentication();

            builder.AddDeveloperSigningCredential();
        }

        public override void PluginConfigure(IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }
    }
}
