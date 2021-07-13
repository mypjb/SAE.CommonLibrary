using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Extension.Middleware;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurationMicrosoftDependencyInjectionExtension
    {
        public static OptionsBuilder<OAuthOptions> AddOAuthClient(this OptionsBuilder<OAuthOptions> optionsBuilder)
        {
            var services = optionsBuilder.Services;

            services.AddOptions();

            services.AddSingleton<IOptionsChangeTokenSource<OAuthOptions>>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                return new ConfigurationChangeTokenSource<OAuthOptions>(optionsBuilder.Name, configuration.GetSection(OAuthOptions.Option));
            });

            services.AddSingleton<IConfigureOptions<OAuthOptions>>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                return new NamedConfigureFromConfigurationOptions<OAuthOptions>(optionsBuilder.Name, configuration.GetSection(OAuthOptions.Option));
            });

            services.TryAddTransient(provider =>
            {
                var optionsMonitor = provider.GetService<IOptionsMonitor<OAuthOptions>>();

                var option = new OAuthOptions();

                option.Extend(optionsMonitor.CurrentValue);

                optionsMonitor.OnChange(op =>
                {
                    option.Extend(op);
                });

                var client = new HttpClient().UseOAuth(option);

                return client;
            });

            return optionsBuilder;
        }

    }
}
