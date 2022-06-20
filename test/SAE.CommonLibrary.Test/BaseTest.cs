using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Test
{
    /// <summary>
    /// base test class 
    /// </summary>
    public abstract class BaseTest
    {
        /// <summary>
        /// console output object
        /// </summary>
        protected readonly ITestOutputHelper _output;
        /// <summary>
        /// service provider 
        /// </summary>
        protected IServiceProvider _serviceProvider;
        /// <summary>
        /// configuration
        /// </summary>
        protected IConfiguration _configuration;
        /// <summary>
        /// default ctor
        /// </summary>
        /// <param name="output"></param>
        public BaseTest(ITestOutputHelper output)
        {
            _output = output;
            IServiceCollection services = new ServiceCollection();
            this._serviceProvider = this.Build(services);
            this._serviceProvider.UseServiceFacade();
            this.Configure(this._serviceProvider);
        }

        /// <summary>
        /// build <see cref="_serviceProvider"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        protected IServiceProvider Build(IServiceCollection services)
        {
            var configurationBuilder = new ConfigurationBuilder();
            this.ConfigureEnvironment(configurationBuilder);
            this.ConfigureConfiguration(configurationBuilder);


            this._configuration = configurationBuilder.Build();
            services.AddSingleton<IConfiguration>(this._configuration);
            services.AddOptions();
            services.AddServiceFacade();

            this.ConfigureServicesBefore(services);
            this.ConfigureServices(services);

            return services.BuildAutofacProvider();
        }

        /// <summary>
        /// add default(json) configuration directory
        /// </summary>
        /// <param name="configurationBuilder"></param>
        public virtual void ConfigureConfiguration(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddJsonFileDirectory();
        }

        /// <summary>
        /// add default(development) env
        /// </summary>
        /// <param name="configurationBuilder"></param>
        protected virtual void ConfigureEnvironment(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                {HostDefaults.EnvironmentKey,Environments.Development },
                {
                    HostDefaults.ApplicationKey,Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName)
                }
            });
        }
        /// <summary>
        /// configure services
        /// </summary>
        /// <param name="services"></param>
        protected virtual void ConfigureServices(IServiceCollection services)
        {
            
        }
        /// <summary>
        /// configure services before
        /// </summary>
        /// <param name="services"></param>
        protected virtual void ConfigureServicesBefore(IServiceCollection services)
        {

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
