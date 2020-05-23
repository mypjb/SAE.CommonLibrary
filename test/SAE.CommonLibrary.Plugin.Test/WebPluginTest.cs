using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Plugin.Constant;
using SAE.CommonLibrary.Test;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Http;

namespace SAE.CommonLibrary.Plugin.Test
{
    public class WebPluginTest : HostTest
    {
        public WebPluginTest(ITestOutputHelper output) : base(output,PluginConstant.Host)
        {
            PluginConstant.HttpMessageHandler = this._httpMessageHandler;
        }



        [Fact]
        public async Task Test()
        {
            var disco = await this._client.GetDiscoveryDocumentAsync(PluginConstant.Host);

            Xunit.Assert.False(disco.IsError, disco.Error);

            // request token
            var tokenResponse = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });

            Xunit.Assert.False(tokenResponse.IsError, tokenResponse.Error);

            this.WriteLine(tokenResponse.Json);

            this._client.SetBearerToken(tokenResponse.AccessToken);

            await this.Request("/weatherforecast");
            await this.Request("/api/weatherforecast");
        }

        private async Task Request(string url)
        {
            var response = await this._client.GetAsync($"{PluginConstant.Host}{url}");
            this.WriteLine(response);

            Xunit.Assert.True(response.IsSuccessStatusCode, ((int)response.StatusCode).ToString());

            var content = await response.Content.ReadAsStringAsync();

            this.WriteLine(content);
        }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNlogLogger();
            services.AddPluginManage("../../../../Plugins/dest");
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthorization();

            app.UsePluginManage();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            
        }
    }
}
