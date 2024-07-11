using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SAE.Framework.Extension;
using SAE.Framework.Logging;
using SAE.Framework.Plugin.Constant;
using SAE.Framework.Test;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace SAE.Framework.Plugin.Test
{
    public class WebMergePluginTest : HostTest
    {
        public static Func<HttpClient> Client;
        public WebMergePluginTest(ITestOutputHelper output) : base(output, PluginConstant.Host)
        {
            this._client.UseLogging(() =>
            {
                return this._serviceProvider.GetService<ILogging<WebMergePluginTest>>();
            });

            Client = () => new HttpClient(new ProxyMessageHandler(this._client));

            // var frontEndType= typeof(SAE.Framework.Plugin.FrontEnd.Startup);
            // var identityType= typeof(SAE.Framework.Plugin.Identity.Startup);
            // var testAPIType= typeof(SAE.Framework.Plugin.TestAPI.Startup);
            // var oAuthType= typeof(SAE.Framework.Plugin.OAuth.OAuthPlugin);
        }


        [Fact]
        public async Task Test()
        {
            var disco = await this._client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = PluginConstant.Host,
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false
                }
            });

            Assert.False(disco.IsError, disco.Error);

            // request token
            var tokenResponse = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });

            Assert.False(tokenResponse.IsError, tokenResponse.Error);

            this.WriteLine(tokenResponse.Json);

            this._client.SetBearerToken(tokenResponse.AccessToken);

            await this.Request("/weatherforecast");
            await this.Request("/api/weatherforecast");
        }

        private async Task Request(string url)
        {
            var response = await this._client.GetAsync($"{PluginConstant.Host}{url}");
            this.WriteLine(response);

            Assert.True(response.IsSuccessStatusCode, ((int)response.StatusCode).ToString());

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
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var auth = this._httpClient.DefaultRequestHeaders.Authorization;

            if (auth != null)
            {
                this._httpClient.DefaultRequestHeaders.Authorization = null;
            }

            var rep = await _httpClient.SendAsync(request.Clone(), cancellationToken);

            if (auth != null)
            {
                this._httpClient.DefaultRequestHeaders.Authorization = auth;
            }
            return rep;
        }
    }

    public class Startup
    {
        public Startup()
        {
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(s =>
            {
                s.SetMinimumLevel(LogLevel.Debug);
            });
            services.AddControllersWithViews();
            services.AddSAEFramework()
                    .AddNlogLogger();

            services.PostConfigureAll<JwtBearerOptions>(options =>
                        {
                            if (WebMergePluginTest.Client.Invoke() == null)
                            {
                                throw new Exception("Handler not exist");
                            }
                            options.Backchannel = WebMergePluginTest.Client.Invoke();
                        });

            services.AddSAEFramework()
                    .AddPluginManage();

            services.PostConfigure<AuthenticationOptions>(s =>
            {
                s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.Use(async (ctx, next) =>
            {
                // var schemeProvider = ctx.RequestServices.GetService<IAuthenticationSchemeProvider>();
                // var schemes = await schemeProvider.GetAllSchemesAsync();
                // var scheme = await schemeProvider.GetDefaultAuthenticateSchemeAsync();
                var user = ctx.User;
                await next.Invoke();
                var t = user;
            });

            app.UsePluginManage();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

        }
    }
}
