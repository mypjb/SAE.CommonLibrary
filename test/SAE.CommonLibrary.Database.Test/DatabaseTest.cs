using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Database;
using SAE.CommonLibrary.Extension;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Test;

public class DatabaseTest : BaseTest
{
    protected readonly IDBConnectionFactory _connectionFactory;

    protected readonly int _maxIndex = new Random().Next(999, 9999);
    public DatabaseTest(ITestOutputHelper output) : base(output)
    {
        this._connectionFactory = this._serviceProvider.GetService<IDBConnectionFactory>();
    }

    protected virtual Dictionary<string, string> GenerateConfiguration()
    {
        var dict = new Dictionary<string, string>();
        Enumerable.Range(0, this._maxIndex)
                  .ForEach(s =>
                  {
                      dict[$"{DBConnectOptions.Option}:{s}:{nameof(DBConnectOptions.Name)}"] = s.ToString();
                      dict[$"{DBConnectOptions.Option}:{s}:{nameof(DBConnectOptions.ConnectionString)}"] = s % 2 == 0 ?
                      $"Data Source=127.0.0.1;Database={s}_mysql;User ID={s}_user;Password={s}_pwd;pooling=true;sslmode=none;CharSet=utf8;allowPublicKeyRetrieval=true" :
                      $"server=127.0.0.1:database={s}_mssql;uid={s}_user;pwd={s}_pwd;";
                      dict[$"{DBConnectOptions.Option}:{s}:{nameof(DBConnectOptions.Provider)}"] = s % 2 == 0 ? "mysql" : "mssql";
                  });
        return dict;
    }
    public override void ConfigureConfiguration(IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.AddInMemoryCollection(this.GenerateConfiguration());
        // base.ConfigureConfiguration(configurationBuilder);
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddMySqlDatabase()
                .AddMSSqlDatabase();

        base.ConfigureServices(services);
    }

    [Fact]
    public virtual void DBTest()
    {
        Enumerable.Range(0, this._maxIndex)
                .AsParallel()
                .ForAll(s =>
                // .ForEach(s =>
                {
                    var dbConnection = this._connectionFactory.GetAsync(s.ToString()).GetAwaiter().GetResult();
                    Xunit.Assert.Contains(s % 2 == 0 ? $"{s}_mysql" : $"{s}_mssql", dbConnection.ConnectionString);
                });

    }
}