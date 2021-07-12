using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using SAE.CommonLibrary.AspNetCore.Routing;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace SAE.CommonLibrary.AspNetCore.Test
{
    public class BitmapAuthTest : HostTest
    {
        public BitmapAuthTest(ITestOutputHelper output) : base(output)
        {
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();
        }
        [Fact]
        public async Task<IEnumerable<IPathDescriptor>> RouterScanningTest()
        {
            var httpResponseMessage = await this._client.GetAsync(Constants.DefaultRoutesPath);
            var descriptors = await httpResponseMessage.AsAsync<IEnumerable<PathDescriptor>>();
            Xunit.Assert.True(descriptors.Any());
            this.WriteLine(descriptors);
            return descriptors;
        }
        [Fact]
        public async Task BitmapAuthorizationTest()
        {
            var pathDescriptors =await this.RouterScanningTest();

            foreach (var descriptor in pathDescriptors)
            {
                var httpResponse = await this._client.GetAsync($"/account/login?path={HttpUtility.UrlEncode(descriptor.Path)}");

                var cookies = httpResponse.Headers.GetValues(HeaderNames.SetCookie);

                var req = new HttpRequestMessage(new HttpMethod(descriptor.Method), descriptor.Path);

                req.Headers.Add(HeaderNames.Cookie, cookies.First());

                var rep = await this._client.SendAsync(req);
                this.WriteLine(descriptor);
                Assert.Equal(System.Net.HttpStatusCode.OK, rep.StatusCode);
            }

        }
        private class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddHttpContextAccessor();
                services.AddControllers(options =>
                {
                    options.Filters.Add(new AuthorizeFilter());
                });

                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie();

                services.AddAuthorization();

                services.AddRoutingScanning()
                        .AddBitmapAuthorization()
                        .AddLocalBitmapEndpointProvider();

            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                app.UseDeveloperExceptionPage();

                app.UseRouting();

                app.UseBitmapAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

                app.UseRoutingScanning();
            }
        }
    }


}
