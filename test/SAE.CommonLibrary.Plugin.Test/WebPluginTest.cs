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
using System;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http;
using System.Threading;

namespace SAE.CommonLibrary.Plugin.Test
{
    public class WebPluginTest : HostTest
    {
        public static Func<HttpMessageHandler> Handler;
        public WebPluginTest(ITestOutputHelper output) : base(output,PluginConstant.Host)
        {
            Handler = () => new ProxyMessageHandler(this._client);
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

    public class ProxyMessageHandler : DelegatingHandler
    {
        private readonly HttpClient _httpClient;

        public ProxyMessageHandler(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _httpClient.SendAsync(request, cancellationToken);
        }
    }

    public class Startup
    {
        private readonly Func<HttpMessageHandler> _handler;

        public Startup()
        {
            this._handler =()=>
            {
                return WebPluginTest.Handler.Invoke();
            };
            
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNlogLogger();
            services.AddPluginManage("../../../../Plugins/dest");
            services.PostConfigureAll<JwtBearerOptions>(options =>
            {
                if (this._handler.Invoke() == null)
                {
                    throw new Exception("Handler not exist");
                }
                options.BackchannelHttpHandler = this._handler.Invoke();
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UsePluginManage();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            
        }
    }
}
