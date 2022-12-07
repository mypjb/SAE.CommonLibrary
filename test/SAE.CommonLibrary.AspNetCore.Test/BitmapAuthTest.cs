using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using SAE.CommonLibrary.AspNetCore.Authorization;
using SAE.CommonLibrary.AspNetCore.Routing;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Test;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace SAE.CommonLibrary.AspNetCore.Test
{
    public class BitmapAuthTest : HostTest
    {
        private readonly List<BitmapAuthorizationDescriptor> _bitmaps = new List<BitmapAuthorizationDescriptor>();
        public BitmapAuthTest(ITestOutputHelper output) : base(output)
        {
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            this.AddProvider(services.AddBitmapAuthorization());
            services.AddNlogLogger();
        }
        protected virtual void AddProvider(BitmapAuthorizationBuilder builder)
        {
            builder.AddLocalBitmapEndpointProvider();
        }

        protected override void Configure(IServiceProvider serviceProvider)
        {
            var bitmapEndpointProvider = serviceProvider.GetService<IBitmapEndpointProvider>();
            if (bitmapEndpointProvider is LocalBitmapEndpointProvider)
            {
                var bitmapEndpoints = bitmapEndpointProvider.ListAsync()
                                                            .GetAwaiter()
                                                            .GetResult();
                foreach (var bitmapEndpoint in bitmapEndpoints.Where(s => s.Path.StartsWith("/noauth")))
                {
                    bitmapEndpoint.Index = 0;
                }
            }

            base.Configure(serviceProvider);
        }

        [Fact]
        public async Task<IEnumerable<IPathDescriptor>> RouterScanningTest()
        {
            var httpResponseMessage = await this._client.GetAsync(Constants.Route.DefaultPath);
            var descriptors = await httpResponseMessage.AsAsync<IEnumerable<PathDescriptor>>();
            Assert.True(descriptors.Any());
            Assert.DoesNotContain(descriptors, s => s.Index == 0);
            this.WriteLine(descriptors);
            return descriptors;
        }
        [Fact]
        public virtual async Task BitmapAuthorizationTest()
        {
            var pathDescriptors = await this.RouterScanningTest();

            foreach (var descriptor in pathDescriptors.Where(s => s.Path.StartsWith("/auth")))
            {
                var httpResponse = await this._client.GetAsync($"/account/login?path={HttpUtility.UrlEncode(descriptor.Path)}&method={descriptor.Method}");

                var cookies = httpResponse.Headers.GetValues(HeaderNames.SetCookie);

                var req = new HttpRequestMessage(new HttpMethod(descriptor.Method), descriptor.Path);

                req.Headers.Add(HeaderNames.Cookie, cookies);

                var rep = await this._client.SendAsync(req);

                if (!rep.IsSuccessStatusCode)
                {
                    this.WriteLine(rep.Headers);
                }

                Assert.Equal(System.Net.HttpStatusCode.OK, rep.StatusCode);
            }

            foreach (var descriptor in pathDescriptors.Where(s => s.Path.StartsWith("/noauth")))
            {
                // var httpResponse = await this._client.GetAsync($"/account/login?path={HttpUtility.UrlEncode(descriptor.Path + "/noauth")}&method={descriptor.Method}");

                // var cookies = httpResponse.Headers.GetValues(HeaderNames.SetCookie);

                var req = new HttpRequestMessage(new HttpMethod(descriptor.Method), descriptor.Path);

                // req.Headers.Add(HeaderNames.Cookie, cookies);

                var rep = await this._client.SendAsync(req);
              
                Assert.Equal(System.Net.HttpStatusCode.OK, rep.StatusCode);
            }


            var adminRep = await this._client.GetAsync($"/account/adminlogin");

            var adminCookies = adminRep.Headers.GetValues(HeaderNames.SetCookie);

            foreach (var descriptor in pathDescriptors.Where(s => s.Path.StartsWith("noauth") || s.Path.StartsWith("auth")))
            {
                var req = new HttpRequestMessage(new HttpMethod(descriptor.Method), descriptor.Path);

                req.Headers.Add(HeaderNames.Cookie, adminCookies);

                var rep = await this._client.SendAsync(req);
                if (!rep.IsSuccessStatusCode)
                {
                    this.WriteLine(rep.Headers);
                }
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

                services.AddBitmapAuthorization();

                services.AddRoutingScanning();
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
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
