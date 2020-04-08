using SAE.CommonLibrary.Plugin.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Plugin.Constant;

namespace SAE.CommonLibrary.Plugin.Test
{
    public class Startup: WebPlugin
    {
        public Startup()
        {
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = PluginConstant.Host;
                        options.BackchannelHttpHandler = PluginConstant.HttpMessageHandler;
                        options.RequireHttpsMetadata = false;

                        options.Audience = "api1";
                    });
            this.PluginConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public  void Configure(IApplicationBuilder app)
        {

            app.UseRouting();
            this.PluginConfigure(app);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public override void PluginConfigureServices(IServiceCollection services)
        {
            
        }

        public override void PluginConfigure(IApplicationBuilder app)
        {

        }
    }
}
