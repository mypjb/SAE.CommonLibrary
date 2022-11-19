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
            builder.UseStartup<Startup>()
                   .ConfigureServices(service =>
                   {
                       this.AddProvider(service.AddBitmapAuthorization());
                   });
        }

        protected virtual void AddProvider(BitmapAuthorizationBuilder builder)
        {
            builder.AddLocalBitmapEndpointProvider();
        }

        protected override void ConfigureConfiguration(IConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConfiguration(configurationBuilder);
            // var dict = new Dictionary<string, string>();

            // Enumerable.Range(0, new Random().Next(1, 999))
            //           .ForEach(s =>
            //           {
            //               var key = $"{BitmapAuthorizationDescriptor.Option}:{s}";
            //               dict[$"{key}:{nameof(BitmapAuthorizationDescriptor.Index)}"] = (s + 1).ToString();
            //               dict[$"{key}:{nameof(BitmapAuthorizationDescriptor.Name)}"] = $"role_{s}";
            //               dict[$"{key}:{nameof(BitmapAuthorizationDescriptor.Description)}"] = "this is a role";
            //               dict[$"{key}:{nameof(BitmapAuthorizationDescriptor.Code)}"] = string.Empty;
            //           });

            // configurationBuilder.AddInMemoryCollection(dict);
        }

        [Fact]
        public async Task<IEnumerable<IPathDescriptor>> RouterScanningTest()
        {
            var httpResponseMessage = await this._client.GetAsync(Constants.Route.DefaultPath);
            var descriptors = await httpResponseMessage.AsAsync<IEnumerable<PathDescriptor>>();
            Assert.True(descriptors.Any());
            //Assert.DoesNotContain(descriptors, s => s.Index == 0);
            this.WriteLine(descriptors);
            return descriptors;
        }
        [Fact]
        public async Task BitmapAuthorizationTest()
        {
            var pathDescriptors = await this.RouterScanningTest();

            foreach (var descriptor in pathDescriptors.Where(s => s.Path.StartsWith("auth")))
            {
                var httpResponse = await this._client.GetAsync($"/account/login?path={HttpUtility.UrlEncode(descriptor.Path)}&method={descriptor.Method}");

                var cookies = httpResponse.Headers.GetValues(HeaderNames.SetCookie);

                var req = new HttpRequestMessage(new HttpMethod(descriptor.Method), descriptor.Path);

                req.Headers.Add(HeaderNames.Cookie, cookies);

                var rep = await this._client.SendAsync(req);
                this.WriteLine(descriptor);
                Assert.Equal(System.Net.HttpStatusCode.OK, rep.StatusCode);
            }

            foreach (var descriptor in pathDescriptors.Where(s => s.Path.StartsWith("noauth")))
            {
                var httpResponse = await this._client.GetAsync($"/account/login?path={HttpUtility.UrlEncode(descriptor.Path + "/noauth")}&method={descriptor.Method}");

                var cookies = httpResponse.Headers.GetValues(HeaderNames.SetCookie);

                var req = new HttpRequestMessage(new HttpMethod(descriptor.Method), descriptor.Path);

                req.Headers.Add(HeaderNames.Cookie, cookies);

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

                services.AddBitmapAuthorization();

                services.AddRoutingScanning();
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
