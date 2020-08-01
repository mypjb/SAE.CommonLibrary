using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
    public class AspNetCoreTest: HostTest
    {
        public AspNetCoreTest(ITestOutputHelper output) : base(output)
        {
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();
        }
        [Fact]
        public async Task<IEnumerable<IPathDescriptor>> RouterScanningTest()
        {
            var httpResponseMessage= await this._client.GetAsync(Constant.DefaultRoutesPath);
            var descriptors =await httpResponseMessage.AsAsync<IEnumerable<PathDescriptor>>();
            Xunit.Assert.True(descriptors.Any());
            this.WriteLine(descriptors);
            return descriptors;
        }
        [Fact]
        public async Task BitmapAuthorizationTest()
        {
            //var descriptors=await this.RouterScanningTest();
            //var descriptor = descriptors.First(s => s.Method.Equals("get", StringComparison.OrdinalIgnoreCase));
            await this._client.GetAsync($"/api/student/display/{Guid.NewGuid()}");
        }
        private class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddHttpContextAccessor();
                services.AddControllers();
                services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
                {
                    options.Filters.Add<Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter>();
                });
                services.AddRoutingScanning()
                        .AddBitmapAuthorization();
                
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                app.UseDeveloperExceptionPage();

                app.UseRouting();


                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

                app.UseRoutingScanning();
            }
        }
    }


}
