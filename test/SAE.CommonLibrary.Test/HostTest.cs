using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Test
{
    /// <summary>
    /// host test base class
    /// </summary>
    public abstract class HostTest : BaseTest
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
        /// ctor
        /// </summary>
        /// <param name="output">console print object</param>
        /// <param name="baseAddress">test client base address</param>
        /// <returns></returns>
        public HostTest(ITestOutputHelper output, string baseAddress) : base(output)
        {
            baseAddress = string.IsNullOrWhiteSpace(baseAddress) ? "http://localhost/" : baseAddress;
            var host = Host.CreateDefaultBuilder()
                           .ConfigureHostConfiguration(this.ConfigureConfiguration)
                           .ConfigureWebHostDefaults(builder =>
                           {
                               builder.UseTestServer();
                               this.ConfigureWebHost(builder);
                           })
                           .UseAutofacProviderFactory()
                           .Build();
            
            host.Start();

            this._serviceProvider = host.Services;

            this.Configure(this._serviceProvider);

            var testServer = host.GetTestServer();

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

        protected virtual void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup(this.GetType().Assembly.GetName().Name);
        }

        protected override void ConfigureEnvironment(IConfigurationBuilder configurationBuilder)
        {
            base.ConfigureEnvironment(configurationBuilder);
        }

    }
}
