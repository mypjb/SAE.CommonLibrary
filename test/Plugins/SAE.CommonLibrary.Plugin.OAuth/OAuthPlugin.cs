using SAE.CommonLibrary.Plugin.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using SAE.CommonLibrary.Plugin.Constant;

namespace SAE.CommonLibrary.Plugin.OAuth
{
    public class OAuthPlugin : WebPlugin
    {
        public override void PluginConfigure(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        public override void PluginConfigureServices(IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new AuthorizeFilter(new[] { new AuthorizeAttribute()}));
            });
            services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = PluginConstant.Host;
                        options.BackchannelHttpHandler = PluginConstant.HttpMessageHandler;
                        options.RequireHttpsMetadata = false;
                        options.Audience = "api1";
                    });
           
        }
    }
}
