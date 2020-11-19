using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Configuration;
using System;

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
            var name = optionsBuilder.Name;
            var services = optionsBuilder.Services;
            services.AddOptions();
            services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                return new ConfigurationChangeTokenSource<TOptions>(Options.Options.DefaultName, configuration.GetSection(optionsBuilder.Name));
            });

            services.AddSingleton<IConfigureOptions<TOptions>>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                return new NamedConfigureFromConfigurationOptions<TOptions>(Options.Options.DefaultName, configuration.GetSection(optionsBuilder.Name), _ => { });
            });
            return optionsBuilder;
        }
        
    }
}
