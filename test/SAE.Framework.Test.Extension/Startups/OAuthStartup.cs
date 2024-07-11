using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.Test.Extension.Startups
{
    public class OAuthStartup
    {
        public  void Configure(IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }

        public  void ConfigureServices(IServiceCollection services)
        {
            var builder = services.AddIdentityServer()
                 .AddInMemoryApiScopes(Config.ApiScopes)
                 .AddInMemoryClients(Config.Clients)
                 .AddJwtBearerClientAuthentication();

            builder.AddDeveloperSigningCredential();

        }
    }
}
