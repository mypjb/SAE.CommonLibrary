using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;
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
            //services.AddServiceProvider();
            this.ConfigureServicesBefore(services);
            this.ConfigureServices(services);
            this._serviceProvider = services.BuildAutofacProvider();
            this.Configure(this._serviceProvider);
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
