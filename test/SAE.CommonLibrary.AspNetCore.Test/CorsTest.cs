using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using SAE.CommonLibrary.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.AspNetCore.Test
{
    public class CorsTest: HostTest
    {
        private const string CorsHost = "https://localhost:9999";
        public CorsTest(ITestOutputHelper output) : base(output)
        {
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();
        }
       
        [Fact]
        public async Task CorsRequestTest()
        {
            var optionsRequest = new HttpRequestMessage(HttpMethod.Options, "/api/home");
            optionsRequest.Headers.Add(HeaderNames.AccessControlRequestMethod, HttpMethod.Post.Method);
            optionsRequest.Headers.Add(HeaderNames.AccessControlRequestHeaders, "Authorization");
            optionsRequest.Headers.Add(HeaderNames.Origin, CorsHost);
            optionsRequest.Headers.Add(HeaderNames.XRequestedWith, "XMLHttpRequest");
            var optionsResponseMessage = await this._client.SendAsync(optionsRequest);
            var origin= optionsResponseMessage.Headers
                                       .GetValues(HeaderNames.AccessControlAllowOrigin)?
                                       .FirstOrDefault();
            Assert.Equal(origin, CorsHost);
            Assert.Equal(optionsRequest.Headers.GetValues(HeaderNames.AccessControlRequestHeaders).First(),
                         optionsResponseMessage.Headers.GetValues(HeaderNames.AccessControlAllowHeaders).First());
            Assert.Equal(optionsRequest.Headers.GetValues(HeaderNames.AccessControlRequestMethod).First(),
                         optionsResponseMessage.Headers.GetValues(HeaderNames.AccessControlAllowMethods).First());
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/home");
            var responseMessage =await this._client.SendAsync(request);
            var json= await responseMessage.Content.ReadAsStringAsync();
            this.WriteLine(json);
            Assert.Contains("value", json);
            
        }
        private class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddControllers();
                services.AddSAECors();

            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                app.UseDeveloperExceptionPage();

                app.UseRouting();

                app.UseSAECors();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            }
        }
    }
}
