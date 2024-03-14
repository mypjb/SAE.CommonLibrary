using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Extension.Middleware;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入配置
    /// </summary>
    public static class ConfigurationMicrosoftDependencyInjectionExtension
    {
        /// <summary>
        /// 注册一个添加OAuth认证的HttpClient
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
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

            services.TryAddSingleton(provider =>
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
