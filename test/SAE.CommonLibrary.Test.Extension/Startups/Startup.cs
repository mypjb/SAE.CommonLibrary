using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Text;

namespace SAE.CommonLibrary.Test.Extension.Startups
{
    public class Startup
    {
        private readonly HttpMessageHandler _httpMessageHandler;

        public Startup(HttpMessageHandler httpMessageHandler)
        {
            this._httpMessageHandler = httpMessageHandler;
        }
        
        public  void ConfigureServices(IServiceCollection services)
        {
            services.AddNlogLogger();
            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.BackchannelHttpHandler = _httpMessageHandler;
                        options.Authority = Config.Authority;
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false
                        };
                    });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Run(async (context) =>
            {
                Encoding encoding = Encoding.UTF8;
                if (context.User.Identity.IsAuthenticated)
                {
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync(context.User.FindFirst(JwtClaimTypes.ClientId).Value, encoding);
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized", encoding);
                }

            });
        }
    }
}
