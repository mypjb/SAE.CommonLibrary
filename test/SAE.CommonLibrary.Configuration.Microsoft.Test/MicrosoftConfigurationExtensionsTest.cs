using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Test;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace SAE.CommonLibrary.Configuration.Microsoft.Test
{
    public class MicrosoftConfigurationExtensionsTest : HostTest
    {
        private const string ConfigPath = "/app/config";
        private const string OfflineConfigPath = "/app/offlineconfig";
        private const int PollInterval = 5;
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

            var includeNode = "Include";

            var host = $"{this._client.BaseAddress.Scheme}://{this._client.BaseAddress.Host}";

            var customNode = "custom";
            var dic = new Dictionary<string, object>
            {
                {DBConnectOptions.Option,databaseOption },
                {
                    includeNode,new object[]
                    {
                        new {
                            name="t1",
                            url=$"{host}{ConfigPath}/t1"
                        },
                        new {
                            name="t2",
                            url=$"{host}{ConfigPath}/t2"
                        },
                        new {
                            name="t3",
                            url=$"{host}{ConfigPath}/t2",
                            nodeName=customNode
                        }
                    }
                }
            };

            var dic1 = new Dictionary<string, string>
            {
                {"t1",this.GetRandom() }
            };

            var dic2 = new Dictionary<string, string>
            {
                {"t2",this.GetRandom() }
            };

            await this.SetConfigAsync(ConfigPath, dic);
            await this.SetConfigAsync($"{ConfigPath}/t1", dic1);
            await this.SetConfigAsync($"{ConfigPath}/t2", dic2);

            var remoteOptions = new SAEOptions
            {
                Url = ConfigPath,
                Client = this._client,
                PollInterval = PollInterval,
                IncludeEndpointConfiguration = includeNode
            };

            var root = this.GetConfigurationBuilder(env)
                           .AddInMemoryCollection(new[] { new KeyValuePair<string, string>(Constants.Config.RootDirectoryKey, Path.Combine(Constants.Config.DefaultRootDirectory, nameof(Remote), env)) })
                           .AddRemoteSource(remoteOptions)
                           .Build();

            var configurationSection = root.GetSection(DBConnectOptions.Option);

            var options = configurationSection.Get<DBConnectOptions>();

            this.Eq(databaseOption, options);

            Assert.Equal(dic1["t1"], root.GetSection("t1").Get<string>());
            Assert.Equal(dic2["t2"], root.GetSection("t2").Get<string>());
            Assert.Equal(dic2["t2"], root.GetSection($"{customNode}:t2").Get<string>());

            databaseOption.Provider = this.GetRandom();
            databaseOption.ConnectionString = this.GetRandom();

            dic1["t1"] = this.GetRandom();
            dic2["t2"] = this.GetRandom();
            await this.SetConfigAsync(ConfigPath, dic);
            await this.SetConfigAsync($"{ConfigPath}/t1", dic1);
            await this.SetConfigAsync($"{ConfigPath}/t2", dic2);

            Thread.Sleep((int)(remoteOptions.PollInterval * 1200));

            options = configurationSection.Get<DBConnectOptions>();

            this.Eq(databaseOption, options);
            Assert.Equal(dic1["t1"], root.GetSection("t1").Get<string>());
            Assert.Equal(dic2["t2"], root.GetSection("t2").Get<string>());
            Assert.Equal(dic2["t2"], root.GetSection($"{customNode}:t2").Get<string>());
        }

        [Theory]
        [InlineData("Development")]
        [InlineData("Production")]
        public async Task CustomRemoteNode(string env)
        {
            var databaseOption = new DBConnectOptions
            {
                ConnectionString = this.GetRandom(),
                Name = this.GetRandom(),
                Provider = this.GetRandom()
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
                PollInterval = PollInterval,
                ConfigurationSection = "Custom.Scope"
            };

            var root = this.GetConfigurationBuilder(env)
                           .AddInMemoryCollection(new[] { new KeyValuePair<string, string>(Constants.Config.RootDirectoryKey, Path.Combine(Constants.Config.DefaultRootDirectory, nameof(CustomRemoteNode), env)) })
                           .AddRemoteSource(remoteOptions)
                           .Build();

            var key = $"{remoteOptions.ConfigurationSection.Replace(Constants.ConfigurationSectionSeparator, ":")}:{DBConnectOptions.Option}";
            var configurationSection = root.GetSection(key);

            var options = configurationSection.Get<DBConnectOptions>();

            this.Eq(databaseOption, options);

            databaseOption.Provider = this.GetRandom();
            databaseOption.ConnectionString = this.GetRandom();

            await this.SetConfigAsync(ConfigPath, dic);

            Thread.Sleep((int)(remoteOptions.PollInterval * 1200));

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
                PollInterval = PollInterval
            };

            var root = this.GetConfigurationBuilder(env)
                           .AddInMemoryCollection(new[] { new KeyValuePair<string, string>(Constants.Config.RootDirectoryKey, Path.Combine(Constants.Config.DefaultRootDirectory, nameof(Remote), env)) })
                           .AddRemoteSource(remoteOptions)
                           .Build();

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

            Thread.Sleep(remoteOptions.PollInterval * 1200);

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
        public DBConnectOptions()
        {
            this.Customs = new[] { "1", "2", "3" };
        }
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
        public string[] Customs { get; set; }
    }
}
