using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.AspNetCore.Routing;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

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
            //var descriptors=await this.RouterScanningTest();
            //var descriptor = descriptors.First(s => s.Method.Equals("get", StringComparison.OrdinalIgnoreCase));
            var responseMessage = await this._client.GetAsync($"/api/student/display/{Guid.NewGuid()}");
            this.WriteLine(responseMessage.Headers);
            this.WriteLine(await responseMessage.Content.ReadAsStringAsync());
            responseMessage.EnsureSuccessStatusCode();
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
