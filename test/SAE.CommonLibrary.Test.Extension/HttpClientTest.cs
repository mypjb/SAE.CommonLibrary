using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using SAE.CommonLibrary.Test.Extension.Startups;
using System;
using System.Linq;
using System.Net.Http;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Test.Extension
{
    public class HttpClientTest : BaseTest
    {
        public HttpClientTest(ITestOutputHelper output) : base(output)
        {
        }

        public HttpClient Initial()
        {
            var oauthHost = Host.CreateDefaultBuilder()
                           .ConfigureJsonFileDirectorySource()
                           .ConfigureWebHostDefaults(builder =>
                           {
                               builder.UseStartup<OAuthStartup>()
                                      .UseTestServer();
                           })
                           .UseAutofacProviderFactory()
                           .Build();
            oauthHost.Start();

            var oauthClient = oauthHost.GetTestClient();

            var apiHost = Host.CreateDefaultBuilder()
                              .ConfigureJsonFileDirectorySource()
                              .ConfigureWebHostDefaults(builder =>
                              {
                                  builder.UseStartup(context =>
                                  {
                                      var startup = new Startup(new ProxyMessageHandler(oauthClient));
                                      return startup;
                                  })
                                         .UseTestServer();
                              })
                              .UseAutofacProviderFactory()
                              .Build();
            apiHost.Start();
            return apiHost.GetTestClient()
                          .UseLoggin(() =>
                          {
                              return apiHost.Services.GetService<ILogging<HttpClientTest>>();
                          })
                          .UseOAuth(new CommonLibrary.Extension.Middleware.OAuthOptions
                          {
                              AppId = "client",
                              AppSecret = "secret",
                              Authority = Config.Authority,
                              Client = oauthClient,
                              Scope = "api1"
                          });
        }
        [Fact]
        public async Task OAuthTest()
        {
            var client = Initial();
            var httpResponse = await client.GetAsync("/api");
            this.WriteLine(await httpResponse.Content.ReadAsStringAsync());
            httpResponse.EnsureSuccessStatusCode();
        }
        //[Fact]
        //public async Task Test()
        //{
        //    var client = new HttpClient().UseOAuth(new CommonLibrary.Extension.Middleware.OAuthOptions
        //    {
        //        AppId = "localhost.test",
        //        AppSecret = "localhost.test",
        //        Authority = "http://oauth.sae.com"
        //    });

        //    //var client = new HttpClient().UseOAuth(new CommonLibrary.Extension.Middleware.OAuthOptions
        //    //{
        //    //    AppId = "localhost.dev",
        //    //    AppSecret = "localhost.dev",
        //    //    Authority = "http://127.0.0.1:8080"
        //    //});
        //    var httpResponse = await client.GetAsync("http://api.sae.com/app/config?id=69cfe444b2a341a5900fad6000c26130&env=Development");
        //    this.WriteLine(await httpResponse.Content.ReadAsStringAsync());
        //} 

        public class ProxyMessageHandler : DelegatingHandler
        {
            private readonly HttpClient _httpClient;

            public ProxyMessageHandler(HttpClient httpClient)
            {
                this._httpClient = httpClient;
            }
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return _httpClient.SendAsync(request.Clone(), cancellationToken);
            }
        }
    }
}
