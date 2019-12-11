using SAE.CommonLibrary.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// 注册服务提供程序<seealso cref="IServiceProvider"/>
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceProvider(IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddSingleton<IServiceProvider>(provider => provider);
            return serviceDescriptors;
        }

        /// <summary>
        /// 设置服务提供程序至<seealso cref="ServiceProvider.Current"/>属性
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceProvider SetServiceProvider(this IServiceProvider serviceProvider)
        {
            ServiceProvider.Current = serviceProvider;
            return serviceProvider;
        }
    }
}
