using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SAE.Framework.Abstract.Authorization.ABAC;
using SAE.Framework.AspNetCore.Authorization.ABAC;
using SAE.Framework.Test;
using Xunit;
using Xunit.Abstractions;

namespace SAE.Framework.AspNetCore.Test
{
    public class ABACAuthorizationTest : HostTest
    {
        private readonly IOptionsMonitor<List<AspNetCoreAuthDescriptor>> _optionsMonitor;

        public ABACAuthorizationTest(ITestOutputHelper output) : base(output)
        {
            this._optionsMonitor = this._serviceProvider.GetService<IOptionsMonitor<List<AspNetCoreAuthDescriptor>>>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();
        }

        [Fact]
        public async Task Success()
        {
            var descriptors = _optionsMonitor.CurrentValue;

            var authDescriptors = descriptors.Where(s => s.Path.StartsWith("/auth")).ToArray();
            var noAuthDescriptors = descriptors.Where(s => s.Path.StartsWith("/noauth")).ToArray();

            var authRep = await this._client.GetAsync($"/abacaccount/login");
            var cookies = authRep.Headers.GetValues(HeaderNames.SetCookie);

            foreach (var descriptor in authDescriptors)
            {
                this.WriteLine($"访问：{descriptor.Method} -> {descriptor.Path}");
                var req = new HttpRequestMessage(new HttpMethod(descriptor.Method), descriptor.Path);

                req.Headers.Add(HeaderNames.Cookie, cookies);

                var rep = await this._client.SendAsync(req);

                if (!rep.IsSuccessStatusCode)
                {
                    this.WriteLine(rep.Headers);
                }

                Assert.Equal(System.Net.HttpStatusCode.OK, rep.StatusCode);
            }

            foreach (var descriptor in noAuthDescriptors)
            {
                this.WriteLine($"访问：{descriptor.Method} -> {descriptor.Path}");
                var path = descriptor.Path;

                if (path.IndexOf('}') != -1 || path.IndexOf(']') != -1)
                {
                    path = $"{path.Substring(0, path.LastIndexOf('/') + 1)}{this.GetRandom()}";
                }

                var req = new HttpRequestMessage(new HttpMethod(descriptor.Method), path);

                var rep = await this._client.SendAsync(req);

                if (!rep.IsSuccessStatusCode)
                {
                    this.WriteLine(rep.Headers);
                }

                Assert.Equal(System.Net.HttpStatusCode.OK, rep.StatusCode);
            }

        }

        [Fact]
        public async Task Fail()
        {
            var descriptors = _optionsMonitor.CurrentValue;

            var authDescriptors = descriptors.Where(s => s.Path.StartsWith("/auth")).ToArray();
            var noAuthDescriptors = descriptors.Where(s => s.Path.StartsWith("/noauth")).ToArray();

            var authRep = await this._client.GetAsync($"/abacaccount/login");
            var cookies = authRep.Headers.GetValues(HeaderNames.SetCookie);

            foreach (var descriptor in authDescriptors)
            {
                var req = new HttpRequestMessage(new HttpMethod(descriptor.Method), descriptor.Path);

                var rep = await this._client.SendAsync(req);

                Assert.NotEqual(System.Net.HttpStatusCode.OK, rep.StatusCode);
            }

            // foreach (var descriptor in noAuthDescriptors)
            // {
            //     var req = new HttpRequestMessage(new HttpMethod(descriptor.Method), descriptor.Path);

            //     var rep = await this._client.SendAsync(req);

            //     Assert.NotEqual(System.Net.HttpStatusCode.OK, rep.StatusCode);
            // }
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

                services.AddSAEFramework()
                        .AddABACAuthorizationWeb();

            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                app.UseRouting();
                app.UseABACAuthorizationWeb();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
        }
    }
}