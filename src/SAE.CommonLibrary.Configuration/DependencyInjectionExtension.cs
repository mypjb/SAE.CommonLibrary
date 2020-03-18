using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Configuration;
using SAE.CommonLibrary.Configuration.Implement;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// 添加默认配置项
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultConfiguration(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddOptionsSource();

            if (!serviceDescriptors.IsRegister<IOptionsProvider>())
            {
                serviceDescriptors.AddOptionsProvider<JsonOptionsProvider>();
                serviceDescriptors.AddOptionsProvider<XmlOptionsProvider>();
            }

            return serviceDescriptors;
        }
        /// <summary>
        /// 添加远程配置项
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddRemoteConfiguration(this IServiceCollection serviceDescriptors, RemoteOptions options)
        {
            options.Check();
            serviceDescriptors.AddSingleton(options);
            serviceDescriptors.AddOptionsProvider<RemoteOptionsProvider>();
            return serviceDescriptors;
        }

        /// <summary>
        /// 注册<seealso cref="IOptionsSource"/>
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddOptionsSource(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.TryAddSingleton<IOptionsSource, OptionsSource>();
            serviceDescriptors.TryAddSingleton(typeof(IOptionsMonitor<>), typeof(OptionsMonitor<>));
            
            return serviceDescriptors;
        }
        /// <summary>
        /// 添加配置源
        /// </summary>
        /// <typeparam name="TOptionsProvider"></typeparam>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddOptionsProvider<TOptionsProvider>(this IServiceCollection serviceDescriptors) where TOptionsProvider : IOptionsProvider
        {
            serviceDescriptors.AddOptionsSource();
            serviceDescriptors.AddSingleton(typeof(IOptionsProvider), typeof(TOptionsProvider));
            return serviceDescriptors;
        }
        /// <summary>
        /// 根据<seealso cref="IOptionsSource"/>添加<typeparamref name="TOptions"/>配置项
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddSaeOptions<TOptions>(this IServiceCollection serviceDescriptors) where TOptions : class, new()
        {
            return serviceDescriptors.AddSaeOptions<TOptions>(typeof(TOptions).Name);
        }
        /// <summary>
        /// 根据<seealso cref="IOptionsSource"/>添加<typeparamref name="TOptions"/>配置项
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="serviceDescriptors"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IServiceCollection AddSaeOptions<TOptions>(this IServiceCollection serviceDescriptors, string name) where TOptions : class, new()
        {
            serviceDescriptors.AddDefaultConfiguration()
                              .TryAddSingleton(provider =>
                              {
                                  var optionsSource = provider.GetService<IOptionsSource>();
                                  var options = optionsSource.Get<TOptions>(name);
                                  return options;
                              });
            
            return serviceDescriptors;
        }
    }
}
