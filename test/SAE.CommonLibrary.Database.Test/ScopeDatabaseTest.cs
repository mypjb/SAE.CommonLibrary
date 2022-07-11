using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Configuration.Microsoft.MultiTenant;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Scope;
using SAE.CommonLibrary.Test;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Database.Test
{
    public class ScopeDatabaseTest : DatabaseTest
    {
        private readonly IScopeFactory _scopeFactory;

        public ScopeDatabaseTest(ITestOutputHelper output) : base(output)
        {
            this._scopeFactory = this._serviceProvider.GetService<IScopeFactory>();
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddDefaultScope()
                    .AddScopeDatabaseFactory()
                    .AddSingleton(typeof(IScopeWrapper<>), typeof(DefaultScopeWrapper<>));
            base.ConfigureServices(services);
        }

        private int GetDbCount(int s)
        {
            return s % 2 == 0 ? 10 : 21;
        }

        protected override Dictionary<string, string> GenerateConfiguration()
        {
            var dict = new Dictionary<string, string>();

            Enumerable.Range(0, this._maxIndex)
                      .ForEach(s =>
                      {
                          var prefix = $"{MultiTenantOptions.Options}:{s}:";
                          Enumerable.Range(0, this.GetDbCount(s))
                                    .ForEach(p =>
                                    {
                                        dict[$"{prefix}{DBConnectOptions.Option}:{p}:{nameof(DBConnectOptions.Name)}"] = s.ToString();
                                        dict[$"{prefix}{DBConnectOptions.Option}:{p}:{nameof(DBConnectOptions.ConnectionString)}"] = p % 2 == 0 ?
                                        $"Data Source=127.0.0.1;Database={p}_mysql;User ID={s}_user;Password={s}_pwd;pooling=true;sslmode=none;CharSet=utf8;allowPublicKeyRetrieval=true" :
                                        $"server=127.0.0.1:database={s}_mssql;uid={s}_user;pwd={s}_pwd;";
                                        dict[$"{prefix}{DBConnectOptions.Option}:{p}:{nameof(DBConnectOptions.Provider)}"] = p % 2 == 0 ? "mysql" : "mssql";
                                    });
                      });
            return dict;
        }


        [Fact]
        public override void DBTest()
        {
            Enumerable.Range(0, this._maxIndex)
                      //   .AsParallel()
                      //   .ForAll(p =>
                      .ForEach(p =>
                      {

                          Enumerable.Range(0, this.GetDbCount(p))
                                    // .AsParallel()
                                    // .ForAll(s =>
                                    .ForEach(s =>
                                    {
                                        using (this._scopeFactory.Get(p.ToString()))
                                        {
                                            var dbConnection = this._connectionFactory.GetAsync(s.ToString()).GetAwaiter().GetResult();
                                            Xunit.Assert.Contains(s % 2 == 0 ? $"{s}_mysql" : $"{s}_mssql", dbConnection.ConnectionString);
                                        }
                                    });
                      });

        }
    }
}