using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Extension;
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
        private readonly IOptions<MultiTenantOptions> _options;

        public AspNetCoreDomainScopeTest(ITestOutputHelper output) : base(output)
        {
            this._options = this._serviceProvider.GetService<IOptions<MultiTenantOptions>>();
            this._scopes = this._options.Value.Mapper.Values.ToArray();
            this.WriteLine(this._options.Value);
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
            return;
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
           await this._scopes
                        // .AsParallel()
                        // .ForAll(async s =>
                        .ForEachAsync(async s =>
                        {
                            var req = this.GetRrequest(HttpMethod.Get, "/home", s);

                            var rep = await this._client.SendAsync(req);
                            rep.EnsureSuccessStatusCode();
                            var current = await rep.Content.ReadAsStringAsync();

                            Assert.Equal(s, current);
                        });
        }

        // [Fact]
        public virtual async Task Switch()
        {
            return;
            this._scopes.AsParallel()
                        .ForAll(async s =>
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

                            var req = this.GetRrequest(HttpMethod.Post, $"/home/switch/{switchScope}", s);

                            var rep = await this._client.SendAsync(req);
                            rep.EnsureSuccessStatusCode();
                            var current = await rep.Content.ReadAsStringAsync();

                            Assert.Equal(s, current);
                        });
        }


        protected virtual HttpRequestMessage GetRrequest(HttpMethod method, string pathString, string scopeName)
        {
            foreach (var kv in this._options.Value.Mapper)
            {
                if (kv.Value.Equals(scopeName))
                {
                    scopeName = kv.Value;
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
                services.AddMultiTenant();
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                app.UseRouting();

                app.UseMultiTenant();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            }
        }
    }
}