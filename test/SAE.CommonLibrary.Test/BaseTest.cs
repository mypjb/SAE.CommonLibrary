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
    public abstract class BaseTest
    {
        protected readonly ITestOutputHelper _output;
        protected IServiceProvider _serviceProvider;
        protected IConfiguration _configuration;
        public BaseTest(ITestOutputHelper output)
        {
            _output = output;
            IServiceCollection services = new ServiceCollection();
            this._serviceProvider = this.Build(services);
            this._serviceProvider.UseServiceFacade();
            this.Configure(this._serviceProvider);
        }

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


        public virtual void ConfigureConfiguration(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddJsonFileDirectory();
        }

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
        protected virtual void ConfigureServices(IServiceCollection services)
        {
            
        }

        protected virtual void ConfigureServicesBefore(IServiceCollection services)
        {

        }

        protected virtual void Configure(IServiceProvider provider)
        {
            
        }

        /// <summary>
        /// 打印输出
        /// </summary>
        /// <param name="object"></param>
        protected void WriteLine(object @object)
        {
            this._output.WriteLine(@object.ToJsonString());
        }
        /// <summary>
        /// 获得随机值
        /// </summary>
        /// <returns></returns>
        protected string GetRandom()
        {
            return Utils.GenerateId().ToMd5(true).ToLower();
        }
    }
}
