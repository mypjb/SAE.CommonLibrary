using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using SAE.CommonLibrary.Extension;
using System;
using System.IO;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Test
{
    public abstract class BaseTest
    {
        protected readonly ITestOutputHelper _output;
        protected readonly IServiceProvider _serviceProvider;
        public BaseTest(ITestOutputHelper output)
        {
            _output = output;
            IServiceCollection services = new ServiceCollection();
            services.AddServiceFacade();
            //services.AddServiceProvider();
            this.ConfigureEnvironment(services);
            this.ConfigureServicesBefore(services);
            this.ConfigureServices(services);
            this._serviceProvider = services.BuildAutofacProvider();
            this._serviceProvider.UseServiceFacade();
            this.Configure(this._serviceProvider);
        }

        protected virtual void ConfigureEnvironment(IServiceCollection services)
        {
            services.AddSingleton<IHostEnvironment>(new HostingEnvironment
            {
                ApplicationName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName),
                EnvironmentName = Environments.Development
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
            return Utils.GenerateId().ToMd5(true);
        }
    }
}
