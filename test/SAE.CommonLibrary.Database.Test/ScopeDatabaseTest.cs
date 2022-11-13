using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Scope;
using SAE.CommonLibrary.Test;
using Xunit.Abstractions;
using Constants = SAE.CommonLibrary.Test.Constants;

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
                          var count = this.GetDbCount(s);
                          var prefix = $"{Constant.Scope}:{s}:";
                          Enumerable.Range(0, count + 1)
                                    .ForEach(p =>
                                    {
                                        var key = $"{prefix}{DBConnectOptions.Option}:{p}";
                                        dict[$"{key}:{nameof(DBConnectOptions.Name)}"] = p.ToString();
                                        if (count == p)
                                        {
                                            var databaseName = $"test_{Guid.NewGuid():N}";
                                            dict[$"{key}:{nameof(DBConnectOptions.ConnectionString)}"] = $"Data Source={Constants.DBConnection.MYSQL.Server};Database={databaseName};User ID={Constants.DBConnection.MYSQL.UId};Password={Constants.DBConnection.MYSQL.Password};pooling=true;port=3306;sslmode=none;CharSet=utf8;allowPublicKeyRetrieval=true";
                                            dict[$"{key}:{nameof(DBConnectOptions.Provider)}"] = "mysql";
                                            dict[$"{key}:{nameof(DBConnectOptions.InitialCommand)}"] = $"CREATE DATABASE {databaseName}";
                                            dict[$"{key}:{nameof(DBConnectOptions.InitialConnectionString)}"] = $"Data Source={Constants.DBConnection.MYSQL.Server};User ID={Constants.DBConnection.MYSQL.UId};Password={Constants.DBConnection.MYSQL.Password};pooling=true;port=3306;sslmode=none;CharSet=utf8;allowPublicKeyRetrieval=true"; ;
                                            dict[$"{key}:{nameof(DBConnectOptions.InitialDetectionCommand)}"] = $"SELECT count(1) FROM information_schema.SCHEMATA WHERE SCHEMA_NAME = '{databaseName}'";
                                        }
                                        else
                                        {
                                            dict[$"{key}:{nameof(DBConnectOptions.ConnectionString)}"] = p % 2 == 0 ?
                                            $"Data Source=127.0.0.1;Database={p}_mysql;User ID={s}_{p};Password={p}_pwd;pooling=true;sslmode=none;CharSet=utf8;allowPublicKeyRetrieval=true" :
                                            $"server=127.0.0.1:database={p}_mssql;uid={s}_{p};pwd={p}_pwd;";
                                            dict[$"{key}:{nameof(DBConnectOptions.Provider)}"] = p % 2 == 0 ? "mysql" : "mssql";
                                        }

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
        [Fact]
        public override Task InitialTest()
        {
            Enumerable.Range(0, this._maxIndex / 100)
                        .AsParallel()
                        // .ForEach(s =>
                        .ForAll(s =>
                        {
                            using (this._scopeFactory.Get(s.ToString()))
                            {
                                var count = this.GetDbCount(s);

                                using (var conn = this._connectionFactory.Get(count.ToString()))
                                {
                                    var key = $"{Constant.Scope}:{s}:{DBConnectOptions.Option}:{count}";
                                    var dBConnectOptions = this._configuration.GetSection(key).Get<DBConnectOptions>();
                                    conn.Open();
                                    using (var command = conn.CreateCommand())
                                    {
                                        command.CommandText = dBConnectOptions.InitialDetectionCommand;
                                        var result = command.ExecuteScalar();
                                        Xunit.Assert.Equal("1", result.ToString());
                                    }
                                }
                            }
                        });

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            Enumerable.Range(0, this._maxIndex / 100)
                        .AsParallel()
                        .ForEach(s =>
                        // .ForAll(s =>
                        {
                            using (this._scopeFactory.Get(s.ToString()))
                            {
                                var count = this.GetDbCount(s);
                                using (var conn = this._connectionFactory.Get(count.ToString()))
                                {
                                    conn.Open();
                                    using (var command = conn.CreateCommand())
                                    {
                                        command.CommandText = $"drop database if exists {conn.Database}";
                                        command.ExecuteNonQuery();
                                    }
                                }
                            }
                        });
        }
    }
}