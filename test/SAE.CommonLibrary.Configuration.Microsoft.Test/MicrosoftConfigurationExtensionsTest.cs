using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SAE.CommonLibrary.Test;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using SAE.CommonLibrary.Extension;
using System;
using System.Threading;

namespace SAE.CommonLibrary.Configuration.Microsoft.Test
{
    public class MicrosoftConfigurationExtensionsTest:HostTest
    {
        public const string ApplicationiName = nameof(ApplicationiName);
        private const string ConfigPath = "/app/config";
        public MicrosoftConfigurationExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }


        [Theory]
        [InlineData("Development")]
        [InlineData("Production")]
        public void JsonFileDirectory(string env)
        {
            var root = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>()
            {
                { HostDefaults.EnvironmentKey,env }
            }).AddJsonFileDirectory().Build();

            var applicationName = root.GetSection(ApplicationiName).Get<string>();
            Xunit.Assert.Equal(applicationName, env);
            if (env.Equals("Development"))
            {
                var custom = root.GetSection("Custom").Get<string>();
                Xunit.Assert.Equal("MSSQL", custom);
            }
        }

        [Fact]
        public async Task Remote()
        {
            var databaseOption = new DBConnectOptions
            {
                ConnectionString = this.GetRandom(),
                Name = this.GetRandom(),
                Provider= this.GetRandom(),
            };

            var dic = new Dictionary<string, object>
            {
                {DBConnectOptions.Option,databaseOption }
            };

            await this.SetConfigAsync(dic);

            var remoteOptions = new SAEOptions
            {
                Url = ConfigPath,
                Client = this._client,
                PollInterval = TimeSpan.FromSeconds(5)
            };

            
            var root = new ConfigurationBuilder().AddRemoteSource(remoteOptions)
                                                 .Build();
            var configurationSection= root.GetSection(DBConnectOptions.Option);
            
            var options = configurationSection.Get<DBConnectOptions>();

            this.Eq(databaseOption, options);


            databaseOption.Provider = this.GetRandom();
            databaseOption.ConnectionString = this.GetRandom();

            await this.SetConfigAsync(dic);

            Thread.Sleep(remoteOptions.PollInterval.Value * 1.2);

            options = configurationSection.Get<DBConnectOptions>();

            this.Eq(databaseOption, options);
        }

        private async Task SetConfigAsync(object data)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, ConfigPath);

            message.AddJsonContent(data);

            await this._client.SendAsync(message);
        }
        private void Eq(DBConnectOptions left,DBConnectOptions right)
        {
            Xunit.Assert.Equal(left.ConnectionString, right.ConnectionString);
            Xunit.Assert.Equal(left.Name, right.Name);
            Xunit.Assert.Equal(left.Provider, right.Provider);
        }

    }

    public class DBConnectOptions
    {
        public const string Option = "database";
        /// <summary>
        /// connection name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// database provider
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// connection string
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
