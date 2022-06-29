using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Scope;
using SAE.CommonLibrary.Test;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace SAE.CommonLibrary.Configuration.Microsoft.Test
{
    public class MulitTenantConfigurationTest : BaseTest
    {
        private const string OptionsName = "op";
        private IScopeFactory _scopeFactory;

        private int EndIndex { get; set; }
        public MulitTenantConfigurationTest(ITestOutputHelper output) : base(output)
        {
            this._scopeFactory = this._serviceProvider.GetService<IScopeFactory>();
        }
        protected override void ConfigureEnvironment(IConfigurationBuilder configurationBuilder, string environmentName = "Development")
        {
            base.ConfigureEnvironment(configurationBuilder, "MulitTenan");
        }
        private string GenericTenantConfiguration(int add)
        {
            this.EndIndex = new Random().Next(5, 10);
            var data = Enumerable.Range(0, this.EndIndex)
                      .Select(s =>
                      {
                          return new KeyValuePair<string, object>(s.ToString(), new
                          {
                              op = new Options
                              {
                                  Data = this.GetRandom(),
                                  Version = s + add
                              }
                          });
                      }).ToDictionary(s => s.Key, s => s.Value);

            var mulitTenantData = new Dictionary<string, object>
            {
              {SAE.CommonLibrary.Constant.Scope,data}
            };

            var filePath = Path.Combine(SAE.CommonLibrary.Constant.Path.Config, "test.MulitTenan.json");

            File.WriteAllText(filePath, mulitTenantData.ToJsonString());

            return filePath;
        }

        public override void ConfigureConfiguration(IConfigurationBuilder configurationBuilder)
        {
            this.GenericTenantConfiguration(0);
            base.ConfigureConfiguration(configurationBuilder);
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddDefaultScope();

            services.AddOptions<Options>()
                    .Bind(OptionsName)
                    .Configure(s =>
                    {
                        s.Version++;
                    });

            base.ConfigureServices(services);
        }

        [Fact]
        public async Task MulitTenantTest()
        {
            var scopes = Enumerable.Range(0, this.EndIndex)
                                   .OrderBy(s => Guid.NewGuid())
                                   .Select(s => s.ToString())
                                   .ToArray();

            var option = this._serviceProvider.GetService<IOptions<Options>>();
            var snapshot = this._serviceProvider.GetService<IOptionsSnapshot<Options>>();
            var monitor = this._serviceProvider.GetService<IOptionsMonitor<Options>>();
            scopes.AsParallel()
                  .ForAll(s =>
            {
                using (var scope = this._scopeFactory.Get(s))
                {
                    Assert.Equal(option.Value, snapshot.Value);
                    Assert.Equal(option.Value, monitor.CurrentValue);
                    Assert.Equal((option.Value.Version - 1).ToString(), s);
                    this.WriteLine(option.Value);
                }
            });

            this.GenericTenantConfiguration(1);

            Thread.Sleep(1000 * 10);

            scopes.AsParallel()
                  .ForAll(s =>
            {
                using (var scope = this._scopeFactory.Get(s))
                {
                    this.WriteLine(new {scope.Name,option.Value});
                    Assert.Equal(option.Value, snapshot.Value);
                    Assert.Equal(option.Value, monitor.CurrentValue);
                    Assert.Equal((option.Value.Version - 2).ToString(), s);
                }
            });
        }
    }
}