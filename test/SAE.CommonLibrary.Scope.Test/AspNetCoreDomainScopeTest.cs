using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Scope.AspNetCore;
using SAE.CommonLibrary.Test;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace SAE.CommonLibrary.Scope.Test
{
    public class AspNetCoreDomainScopeTest : HostTest
    {
        protected IEnumerable<string> _scopes;
        protected readonly IOptions<MultiTenantOptions> _options;

        public AspNetCoreDomainScopeTest(ITestOutputHelper output) : base(output)
        {
            this._options = this._serviceProvider.GetService<IOptions<MultiTenantOptions>>();
            this._scopes = this._options.Value.Mapper.Values.ToArray();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();
        }

        protected virtual IEnumerable<string> GenerateScopes()
        {
            return Enumerable.Range(0, new Random().Next(999, 9999)).Select(s => $"{s}_t").ToArray();
        }

        public override void ConfigureConfiguration(IConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConfiguration(configurationBuilder);

            var scopes = this.GenerateScopes()
                             .ToDictionary(s =>
                             {
                                 return $"{MultiTenantOptions.Option}:{nameof(MultiTenantOptions.Mapper)}:{s}_domain";
                             }, s => s);


            configurationBuilder.AddInMemoryCollection(scopes);
        }
        protected override void ConfigureEnvironment(IHostBuilder builder, string environmentName = "Development")
        {
            base.ConfigureEnvironment(builder, "domain");
        }

        [Fact]
        public virtual async Task Get()
        {
            var tasks = this._scopes.Select(async s =>
                       {
                           var req = await this.GetRrequestAsync(HttpMethod.Get, "/home", s);
                           var rep = await this._client.SendAsync(req);
                           rep.EnsureSuccessStatusCode();
                           var current = await rep.Content.ReadAsStringAsync();
                           Assert.Equal(s, current);
                       });
            await Task.WhenAll(tasks);
        }

        [Fact]
        public virtual async Task Switch()
        {
            var tasks = this._scopes.Select(async s =>
                        {
                            var switchScope = string.Empty;
                            while (true)
                            {
                                var find = this._scopes.ElementAt(Math.Abs(this.GetRandom().GetHashCode()) % this._scopes.Count());
                                if (!find.Equals(s))
                                {
                                    switchScope = find;
                                    break;
                                }
                            }

                            var req = await this.GetRrequestAsync(HttpMethod.Post, $"/home/switch/{switchScope}", s);

                            var rep = await this._client.SendAsync(req);
                            rep.EnsureSuccessStatusCode();
                            var current = await rep.Content.ReadAsStringAsync();

                            Assert.Equal(switchScope, current);
                        });
            await Task.WhenAll(tasks);
        }


        protected virtual async Task<HttpRequestMessage> GetRrequestAsync(HttpMethod method, string pathString, string scopeName)
        {
            foreach (var kv in this._options.Value.Mapper)
            {
                if (kv.Value.Equals(scopeName))
                {
                    scopeName = kv.Key;
                    break;
                }
            }

            var req = new HttpRequestMessage(method, $"http://{scopeName}.{this._client.BaseAddress.Host}{pathString}");

            return req;
        }

        private class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddControllers();
                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie();
                services.AddMultiTenant();

            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();
                app.UseMultiTenant();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            }
        }
    }
}