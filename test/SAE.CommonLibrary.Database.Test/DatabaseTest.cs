using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Database;
using SAE.CommonLibrary.Extension;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Test;

public class DatabaseTest : BaseTest, IDisposable
{
    protected readonly IDBConnectionFactory _connectionFactory;

    protected readonly int _maxIndex = new Random().Next(1111, 2222);
    public DatabaseTest(ITestOutputHelper output) : base(output)
    {
        this._connectionFactory = this._serviceProvider.GetService<IDBConnectionFactory>();
    }

    protected virtual Dictionary<string, string> GenerateConfiguration()
    {
        var dict = new Dictionary<string, string>();
        Enumerable.Range(0, this._maxIndex + 1)
                  .ForEach(s =>
                  {
                      var key = $"{DBConnectOptions.Option}:{s}";
                      dict[$"{key}:{nameof(DBConnectOptions.Name)}"] = s.ToString();
                      if (s == this._maxIndex)
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
                          dict[$"{key}:{nameof(DBConnectOptions.ConnectionString)}"] = s % 2 == 0 ?
                          $"Data Source=127.0.0.1;Database={s}_mysql;User ID={s}_user;Password={s}_pwd;pooling=true;sslmode=none;CharSet=utf8;allowPublicKeyRetrieval=true" :
                          $"server=127.0.0.1:database={s}_mssql;uid={s}_user;pwd={s}_pwd;";
                          dict[$"{key}:{nameof(DBConnectOptions.Provider)}"] = s % 2 == 0 ? "mysql" : "mssql";
                      }

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
                .AddMSSqlDatabase()
                .AddNlogLogger();

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

    [Fact]
    public virtual async Task InitialTest()
    {
        var key = $"{DBConnectOptions.Option}:{this._maxIndex}";
        var dBConnectOptions = this._configuration.GetSection(key).Get<DBConnectOptions>();
        using (var conn = await this._connectionFactory.GetAsync(this._maxIndex.ToString()))
        {
            conn.Open();
            using (var command = conn.CreateCommand())
            {
                command.CommandText = dBConnectOptions.InitialDetectionCommand;
                var result = command.ExecuteScalar();
                Xunit.Assert.Equal("1", result.ToString());
            }
        }
    }

    /// <summary>
    /// 删除数据库
    /// </summary>
    public virtual void Dispose()
    {
        using (var conn = this._connectionFactory.Get(this._maxIndex.ToString()))
        {
            conn.Open();
            using (var command = conn.CreateCommand())
            {
                command.CommandText = $"drop database if exists {conn.Database}";
                command.ExecuteNonQuery();
            }
        }
    }
}