using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Configuration;
using System;

namespace Microsoft.Extensions.Hosting
{
    public static class ConfigurationMicrosoftHostBuilderExtensions
    {
        public static IHostBuilder ConfigureSAEConfiguration(this IHostBuilder builder, Action<SAEOptions> action)
        {
            builder.ConfigureHostConfiguration(conf =>
            {
                conf.AddSAEConfiguration(action);
            });
            return builder;
        }

        public static IHostBuilder ConfigureSAEConfiguration(this IHostBuilder builder, SAEOptions options)
        {
            builder.ConfigureHostConfiguration(conf =>
            {
                conf.AddSAEConfiguration(options);
            });
            return builder;
        }

        public static IHostBuilder ConfigureSAEConfiguration(this IHostBuilder builder, string url)
        {
            builder.ConfigureHostConfiguration(conf =>
            {
                conf.AddSAEConfiguration(url);
            });
            return builder;
        }
    }
}
