using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SAE.Framework.Extension;
using Xunit.Abstractions;

namespace SAE.Framework.Test
{
    /// <summary>
    /// host test base class
    /// </summary>
    public abstract class HostTest
    {
        /// <summary>
        /// test client
        /// </summary>
        protected readonly HttpClient _client;
        /// <summary>
        /// test client handler
        /// </summary>
        protected readonly HttpMessageHandler _httpMessageHandler;
        /// <summary>
        /// service provider
        /// </summary>
        protected readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// console print tool
        /// </summary>
        protected readonly ITestOutputHelper _output;


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="output">console print object</param>
        /// <param name="baseAddress">test client base address</param>
        /// <returns></returns>
        public HostTest(ITestOutputHelper output, string baseAddress)
        {
            this._output = output;

            baseAddress = string.IsNullOrWhiteSpace(baseAddress) ? "http://localhost/" : baseAddress;

            var host = Host.CreateDefaultBuilder();
            this.ConfigureEnvironment(host);
            var build = host.ConfigureHostConfiguration(this.ConfigureConfiguration)
                            .ConfigureWebHostDefaults(builder =>
                            {
                                builder.UseTestServer();
                                this.ConfigureWebHost(builder);
                                builder.ConfigureServices((host, services) =>
                                {
                                    this.ConfigureServices(services);
                                });
                            })
                            .UseAutofacProviderFactory()
                            .Build();

            build.Start();

            this._serviceProvider = build.Services;

            this.Configure(this._serviceProvider);

            var testServer = build.GetTestServer();

            this._httpMessageHandler = testServer.CreateHandler();

            this._client = new HttpClient(this._httpMessageHandler) { BaseAddress = new Uri(baseAddress) };

        }


        /// <summary>
        /// default ctor
        /// </summary>
        /// <param name="output"></param>
        public HostTest(ITestOutputHelper output) : this(output, "http://localhost/")
        {

        }

        /// <summary>
        /// add default(json) configuration directory
        /// </summary>
        /// <param name="configurationBuilder"></param>
        protected virtual void ConfigureConfiguration(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddJsonFileDirectory();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        protected virtual void ConfigureServices(IServiceCollection services)
        {

        }

        /// <summary>
        /// add default(development) env
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="environmentName"></param>
        protected virtual void ConfigureEnvironment(IHostBuilder builder, string environmentName = nameof(Environments.Development))
        {
            builder.UseEnvironment(environmentName);
        }

        /// <summary>
        /// configuration web host
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup(this.GetType().Assembly.GetName().Name);
        }

        /// <summary>
        /// configure <see cref="_serviceProvider"/>
        /// </summary>
        /// <param name="provider"></param>
        protected virtual void Configure(IServiceProvider provider)
        {

        }

        /// <summary>
        /// print <paramref name="object"/> to console
        /// </summary>
        /// <param name="object"></param>
        protected void WriteLine(object @object)
        {
            this._output.WriteLine(@object.ToJsonString());
        }
        /// <summary>
        /// get random
        /// </summary>
        /// <returns></returns>
        protected string GetRandom()
        {
            return Utils.GenerateId().ToMd5(true).ToLower();
        }
    }
}
