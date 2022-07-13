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
            services.AddDefaultScope();
            // .AddScopeDatabaseFactory()
            // .AddSingleton(typeof(IScopeWrapper<>), typeof(DefaultScopeWrapper<>));
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
                          var prefix = $"{Constant.Scope}:{s}:";
                          Enumerable.Range(0, this.GetDbCount(s))
                                    .ForEach(p =>
                                    {
                                        dict[$"{prefix}{DBConnectOptions.Option}:{p}:{nameof(DBConnectOptions.Name)}"] = p.ToString();
                                        dict[$"{prefix}{DBConnectOptions.Option}:{p}:{nameof(DBConnectOptions.ConnectionString)}"] = p % 2 == 0 ?
                                        $"Data Source=127.0.0.1;Database={p}_mysql;User ID={s}_{p};Password={p}_pwd;pooling=true;sslmode=none;CharSet=utf8;allowPublicKeyRetrieval=true" :
                                        $"server=127.0.0.1:database={p}_mssql;uid={s}_{p};pwd={p}_pwd;";
                                        dict[$"{prefix}{DBConnectOptions.Option}:{p}:{nameof(DBConnectOptions.Provider)}"] = p % 2 == 0 ? "mysql" : "mssql";
                                    });
                      });
            return dict;
        }


        [Fact]
        public override void DBTest()
        {
            Enumerable.Range(0, this._maxIndex)
                        .AsParallel()
                        .ForAll(s =>
                    //   .ForEach(s =>
                      {

                          Enumerable.Range(0, this.GetDbCount(s))
                                    .AsParallel()
                                    .ForAll(p =>
                                    // .ForEach(p =>
                                    {
                                        using (this._scopeFactory.Get(s.ToString()))
                                        {
                                            var configuration = this._configuration;
                                            var dbConnection = this._connectionFactory.GetAsync(p.ToString()).GetAwaiter().GetResult();
                                            Xunit.Assert.Contains(p % 2 == 0 ? $"{p}_mysql" : $"{p}_mssql", dbConnection.ConnectionString);
                                            Xunit.Assert.Contains($"{s}_{p}", dbConnection.ConnectionString);
                                        }
                                    });
                      });

        }
    }
}