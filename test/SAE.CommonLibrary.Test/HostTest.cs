using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Test
{
    public abstract class HostTest : BaseTest
    {
        protected readonly HttpClient _client;
        protected readonly HttpMessageHandler _httpMessageHandler;
        public HostTest(ITestOutputHelper output, string baseAddress) : base(output)
        {
            baseAddress = string.IsNullOrWhiteSpace(baseAddress) ? "http://localhost/" : baseAddress;
            var host = Host.CreateDefaultBuilder()
                           .ConfigureServices((ctx, s) =>
                           {

                           })
                           .ConfigureJsonFileDirectorySource()
                           .ConfigureWebHostDefaults(builder =>
                           {
                               builder.UseTestServer();
                               this.ConfigureWebHost(builder);
                           })
                           .UseAutofacProviderFactory()
                           .Build();
            host.Start();

            this._serviceProvider = host.Services;

            var testServer = host.GetTestServer();

            this._httpMessageHandler = testServer.CreateHandler();

            this._client = new HttpClient(this._httpMessageHandler) { BaseAddress = new Uri(baseAddress) };
        }

        public HostTest(ITestOutputHelper output) : this(output, "http://localhost/")
        {

        }

        protected virtual void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup(this.GetType().Assembly.GetName().Name);
        }

    }
}
