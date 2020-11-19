using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Configuration;
using System;

namespace Microsoft.Extensions.Hosting
{
    public static class ConfigurationMicrosoftHostBuilderExtensions
    {
        /// <summary>
        /// Add SAE configuration source
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureRemoteSource(this IHostBuilder builder, Action<SAEOptions> action)
        {
            builder.ConfigureHostConfiguration(conf =>
            {
                conf.AddRemoteSource(action);
            });
            return builder;
        }
        /// <summary>
        /// Add remote configuration source
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureRemoteSource(this IHostBuilder builder, SAEOptions options)
        {
            builder.ConfigureHostConfiguration(conf =>
            {
                conf.AddRemoteSource(options);
            });
            return builder;
        }
        /// <summary>
        /// Add remote configuration source
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureRemoteSource(this IHostBuilder builder, string url)
        {
            builder.ConfigureHostConfiguration(conf =>
            {
                conf.AddRemoteSource(url);
            });
            return builder;
        }
        /// <summary>
        /// Add <seealso cref="MicrosoftConfigurationExtensions.DefaultConfigDirectory"/>  directory As a configuration source
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureJsonFileDirectorySource(this IHostBuilder builder)
        {
            return builder.ConfigureJsonFileDirectorySource(MicrosoftConfigurationExtensions.DefaultConfigDirectory);
        }
        /// <summary>
        /// Add <paramref name="path"/>  directory As a configuration source
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="path">json file directory</param>
        /// <returns></returns>
        public static IHostBuilder ConfigureJsonFileDirectorySource(this IHostBuilder builder,string path)
        {
            builder.ConfigureHostConfiguration(conf =>
            {
                conf.AddJsonFileDirectory(path);
            });
            return builder;
        }


    }
}
