using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Configuration;
using SAE.CommonLibrary.Configuration.Microsoft;
using SAE.CommonLibrary.Configuration.Microsoft.MultiTenant;
using SAE.CommonLibrary.DependencyInjection;
using SAE.CommonLibrary.Scope;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 配置依赖管理
    /// </summary>
    public static class ConfigurationMicrosoftDependencyInjectionExtension
    {

        // /// <summary>
        // /// 添加配置管理
        // /// </summary>
        // /// <typeparam name="TOptions"></typeparam>
        // /// <typeparam name="TService"></typeparam>
        // public static IServiceCollection AddOptionsManage<TOptions, TService>(this IServiceCollection services) where TOptions : class where TService : class
        // {
        //     services.AddTransient<IOptionsManage<TOptions, TService>, OptionsManager<TOptions, TService>>();
        //     return services;
        // }
        // /// <summary>
        // /// 添加配置 <see cref="IOptions{TOptions}"/>
        // /// </summary>
        // /// <typeparam name="TOptions"></typeparam>
        // /// <typeparam name="TService"></typeparam>
        // public static OptionsBuilder<TOptions> ConfigureService<TOptions, TService>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class where TService : class
        // {
        //     optionsBuilder.Services.AddOptionsManage<TOptions, TService>();
        //     return optionsBuilder;
        // }

        /// <summary>
        /// 使用<see cref="IConfiguration"/>进行配置绑定
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static OptionsBuilder<TOptions> Bind<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
        {
            return optionsBuilder.Bind(Options.Options.DefaultName);
        }

        /// <summary>
        /// 将<see cref="IConfiguration"/>的<paramref name="key"/>绑定到配置
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="optionsBuilder"></param>
        /// <param name="key"><see cref="IConfiguration"/> section</param>
        /// <returns></returns>
        public static OptionsBuilder<TOptions> Bind<TOptions>(this OptionsBuilder<TOptions> optionsBuilder, string key) where TOptions : class
        {

            var services = optionsBuilder.Services;

            var configurationName = optionsBuilder.Name;

            services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                return new ConfigurationChangeTokenSource<TOptions>(configurationName, configuration.GetSection(key));
            });

            if (services.IsRegister<IScopeFactory>())
            {
                var configuration = services.FindConfiguration();


                services.AddOptions<MultiTenantOptions<TOptions>>(optionsBuilder.Name)
                        .Bind(configuration.GetSection(MultiTenantOptions.Options))
                        .Configure(p =>
                        {
                            p.Key = key;
                            p.Name = configurationName;
                        });

                services.TryAddSingleton<IOptions<TOptions>, MultiTenantUnnamedOptionsManager<TOptions>>();
                services.TryAddSingleton<IOptionsSnapshot<TOptions>, MultiTenantOptionsManager<TOptions>>();
                services.TryAddSingleton<IOptionsMonitor<TOptions>, MultiTenantOptionsMonitor<TOptions>>();
                services.TryAddSingleton<IOptionsFactory<TOptions>, MultiTenantOptionsFactory<TOptions>>();
            }
            else
            {
                services.AddSingleton<IConfigureOptions<TOptions>>(provider =>
                {
                    var configuration = provider.GetService<IConfiguration>();
                    return new NamedConfigureFromConfigurationOptions<TOptions>(configurationName, configuration.GetSection(key), _ => { });
                });
            }


            return optionsBuilder;
        }

        /// <summary>
        /// 查找配置 <see cref="IConfiguration"/> 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IConfiguration FindConfiguration(this IServiceCollection services)
        {
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IConfiguration));

            if (serviceDescriptor == null)
            {
                return null;
            }

            if (serviceDescriptor.ImplementationInstance == null)
            {
                var serviceProvider = services.BuildServiceProvider();
                if (serviceProvider.TryGetService<IConfiguration>(out var configuration))
                {
                    return configuration;
                }
            }

            return (IConfiguration)serviceDescriptor.ImplementationInstance;
        }


    }
}
