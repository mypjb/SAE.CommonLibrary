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
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Assert = Xunit.Assert;

namespace SAE.CommonLibrary.Configuration.Microsoft.Test
{
    public class MicrosoftConfigurationExtensionsTest : HostTest
    {
        private const string ConfigPath = "/app/config";
        private const string OfflineConfigPath = "/app/offlineconfig";
        private const int PollInterval = 2;
        public MicrosoftConfigurationExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddTinyMapper();
            base.ConfigureServices(services);
        }

        [Theory]
        [InlineData("Development")]
        [InlineData("Production")]
        public void JsonFileDirectory(string env)
        {
            var root = this.GetConfigurationBuilder(env).AddJsonFileDirectory().Build();

            Xunit.Assert.Equal(root.GetSection(HostDefaults.EnvironmentKey).Get<string>(), env);

            if (env.Equals("Development"))
            {
                var custom = root.GetSection("Custom").Get<string>();
                Xunit.Assert.Equal("MSSQL", custom);
            }
        }

        [Theory]
        [InlineData("Development")]
        [InlineData("Production")]
        public async Task Remote(string env)
        {
            var databaseOption = new DBConnectOptions
            {
                ConnectionString = this.GetRandom(),
                Name = this.GetRandom(),
                Provider = this.GetRandom(),
            };

            var dic = new Dictionary<string, object>
            {
                {DBConnectOptions.Option,databaseOption }
            };

            await this.SetConfigAsync(ConfigPath, dic);

            var remoteOptions = new SAEOptions
            {
                Url = ConfigPath,
                Client = this._client,
                PollInterval = TimeSpan.FromSeconds(PollInterval)
            };

            var root = this.GetConfigurationBuilder(env).AddRemoteSource(remoteOptions).Build();

            var configurationSection = root.GetSection(DBConnectOptions.Option);

            var options = configurationSection.Get<DBConnectOptions>();

            this.Eq(databaseOption, options);


            databaseOption.Provider = this.GetRandom();
            databaseOption.ConnectionString = this.GetRandom();

            await this.SetConfigAsync(ConfigPath, dic);

            Thread.Sleep(remoteOptions.PollInterval.Value * 1.2);

            options = configurationSection.Get<DBConnectOptions>();

            this.Eq(databaseOption, options);

        }

        [Theory]
        [InlineData("Development")]
        [InlineData("Production")]
        public async Task Offline(string env)
        {
            await this.Remote(env);

            var remoteOptions = new SAEOptions
            {
                Url = OfflineConfigPath,
                Client = this._client,
                PollInterval = TimeSpan.FromSeconds(PollInterval)
            };

            var root = this.GetConfigurationBuilder(env).AddRemoteSource(remoteOptions).Build();

            var configurationSection = root.GetSection(DBConnectOptions.Option);

            Assert.True(configurationSection.Exists());

            var options = configurationSection.Get<DBConnectOptions>();

            this.WriteLine(options);

            var databaseOption = new DBConnectOptions
            {
                ConnectionString = this.GetRandom(),
                Name = this.GetRandom(),
                Provider = this.GetRandom(),
            };

            var dic = new Dictionary<string, object>
            {
                {DBConnectOptions.Option,databaseOption }
            };

            await this.SetConfigAsync(OfflineConfigPath, dic);

            Thread.Sleep(remoteOptions.PollInterval.Value * 1.2);

            options = configurationSection.Get<DBConnectOptions>();

            this.Eq(databaseOption, options);
        }


        private async Task SetConfigAsync(string url, object data)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, url);

            message.AddJsonContent(data);

            var httpResponse = await this._client.SendAsync(message);
            httpResponse.EnsureSuccessStatusCode();
        }
        private void Eq(DBConnectOptions left, DBConnectOptions right)
        {
            Xunit.Assert.Equal(left.ConnectionString, right.ConnectionString);
            Xunit.Assert.Equal(left.Name, right.Name);
            Xunit.Assert.Equal(left.Provider, right.Provider);
        }


        private IConfigurationBuilder GetConfigurationBuilder(string env)
        {
            var dic = new Dictionary<string, string>()
            {
                {
                    HostDefaults.EnvironmentKey,
                    env
                },
                {
                    HostDefaults.ApplicationKey,
                    Assembly.GetExecutingAssembly().GetName().Name
                }
            };

            //if (pairs != null && pairs.Any())
            //{
            //    foreach (var kv in pairs)
            //    {
            //        dic[kv.Key] = kv.Value;
            //    }
            //}

            return new ConfigurationBuilder().AddInMemoryCollection(dic);
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
