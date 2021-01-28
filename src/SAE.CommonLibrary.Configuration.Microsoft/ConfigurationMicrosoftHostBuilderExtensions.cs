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
            return builder.ConfigureHostConfiguration(conf =>
            {
                conf.AddRemoteSource(action);
            });
        }
        /// <summary>
        /// Add remote configuration source
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureRemoteSource(this IHostBuilder builder, SAEOptions options)
        {
            return builder.ConfigureHostConfiguration(conf =>
            {
                conf.AddRemoteSource(options);
            });
        }
        /// <summary>
        /// Add remote configuration source
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureRemoteSource(this IHostBuilder builder, string url)
        {
            return builder.ConfigureHostConfiguration(conf =>
            {
                conf.AddRemoteSource(url);
            });
        }
        /// <summary>
        /// Add <seealso cref="MicrosoftConfigurationExtensions.DefaultConfigDirectory"/>  directory As a configuration source
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureJsonFileDirectorySource(this IHostBuilder builder)
        {
            return builder.ConfigureJsonFileDirectorySource(null);
        }
        /// <summary>
        /// Add <paramref name="path"/>  directory As a configuration source
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="path">json file directory</param>
        /// <returns></returns>
        public static IHostBuilder ConfigureJsonFileDirectorySource(this IHostBuilder builder,string path)
        {
            return builder.ConfigureHostConfiguration(conf =>
            {
                conf.AddJsonFileDirectory(path);
            });
        }


    }
}
