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
using SAE.Framework.Extension;
using SAE.Framework.Scope;
using SAE.Framework.Test;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace SAE.Framework.Configuration.Microsoft.Test
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
            base.ConfigureEnvironment(configurationBuilder, "MulitTenant");
        }
        private string GenericTenantConfiguration(int add)
        {
            if (this.EndIndex <= 0)
            {
                this.EndIndex = new Random().Next(999, 9999);

            }

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
              {SAE.Framework.Constants.Scope,data}
            };

            var filePath = Path.Combine(SAE.Framework.Constants.Path.Config, "test.MulitTenant.json");

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
            services.AddSAEFramework()
                    .AddDefaultScope();

            services.AddOptions<Options>()
                    .Bind(OptionsName)
                    .Configure(s =>
                    {
                        s.Version++;
                    });

            services.AddOptions<DBConnectOptions>()
                    .Bind(DBConnectOptions.Option);

            base.ConfigureServices(services);
        }

        /// <summary>
        /// 
        /// </summary>
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
            var dBConnect = this._serviceProvider.GetService<IOptions<DBConnectOptions>>().Value;
            
            scopes.AsParallel()
                  .ForAll(s =>
            {
                using (var scope = this._scopeFactory.Get(s))
                {
                    Assert.Equal(option.Value, snapshot.Value);
                    Assert.Equal(option.Value, monitor.CurrentValue);
                    Assert.Equal((option.Value.Version - 1).ToString(), s);
                    this.WriteLine(option.Value);
                    var dBConnectOptions = this._serviceProvider.GetService<IOptions<DBConnectOptions>>();
                    Assert.Equal(dBConnect.Name,dBConnectOptions.Value.Name);
                    Assert.Equal(dBConnect.ConnectionString,dBConnectOptions.Value.ConnectionString);
                    Assert.Equal(dBConnect.Provider,dBConnectOptions.Value.Provider);
                    Assert.Equal(dBConnect.Customs,dBConnectOptions.Value.Customs);
                }
            });

            this.GenericTenantConfiguration(1);

            Thread.Sleep(1000 * 5);

            scopes.AsParallel()
                  .ForAll(s =>
            {
                using (var scope = this._scopeFactory.Get(s))
                {
                    this.WriteLine(new { scope.Name, option.Value });
                    Assert.Equal(option.Value, snapshot.Value);
                    Assert.Equal(option.Value, monitor.CurrentValue);
                    Assert.Equal((option.Value.Version - 2).ToString(), s);
                    var dBConnectOptions = this._serviceProvider.GetService<IOptions<DBConnectOptions>>();
                    Assert.Equal(dBConnect.Name,dBConnectOptions.Value.Name);
                    Assert.Equal(dBConnect.ConnectionString,dBConnectOptions.Value.ConnectionString);
                    Assert.Equal(dBConnect.Provider,dBConnectOptions.Value.Provider);
                    Assert.Equal(dBConnect.Customs,dBConnectOptions.Value.Customs);
                }
            });
            this.WriteLine(dBConnect);
        }
    }
}