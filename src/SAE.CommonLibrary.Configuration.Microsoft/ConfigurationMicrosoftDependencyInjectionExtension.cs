using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Configuration;
using SAE.CommonLibrary.Configuration.Microsoft.MultiTenant;
using SAE.CommonLibrary.DependencyInjection;
using SAE.CommonLibrary.Scope;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurationMicrosoftDependencyInjectionExtension
    {
        /// <summary>
        /// From <see cref="IConfiguration"/> binding
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static OptionsBuilder<TOptions> Bind<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
        {
            return optionsBuilder.Bind(Options.Options.DefaultName);
        }

        /// <summary>
        /// Use <paramref name="key"/> bind <see cref="IConfiguration"/>
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

            services.AddSingleton<IConfigureOptions<TOptions>>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                return new NamedConfigureFromConfigurationOptions<TOptions>(configurationName, configuration.GetSection(key), _ => { });
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


            return optionsBuilder;
        }

        // <summary>
        /// Find <see cref="IConfiguration"/> 
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
