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
            return optionsBuilder.Bind(Options.Options.DefaultName);
        }

        /// <summary>
        /// Use <paramref name="key"/> bind <see cref="IConfiguration"/>
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="optionsBuilder"></param>
        /// <param name="key"><see cref="IConfiguration"/> section</param>
        /// <returns></returns>
        public static OptionsBuilder<TOptions> Bind<TOptions>(this OptionsBuilder<TOptions> optionsBuilder,string key) where TOptions : class
        {
            var services = optionsBuilder.Services;
            services.AddOptions();
            services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                return new ConfigurationChangeTokenSource<TOptions>(optionsBuilder.Name, configuration.GetSection(key));
            });

            services.AddSingleton<IConfigureOptions<TOptions>>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                return new NamedConfigureFromConfigurationOptions<TOptions>(optionsBuilder.Name, configuration.GetSection(key), _ => { });
            });
            return optionsBuilder;
        }


    }
}
