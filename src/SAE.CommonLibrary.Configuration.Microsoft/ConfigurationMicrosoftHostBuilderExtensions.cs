using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Configuration;
using System;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigurationMicrosoftHostBuilderExtensions
    {
        /// <summary>
        /// add sae configuration source
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureRemoteSource(this IHostBuilder builder, Action<SAEOptions> action)
        {
            return builder.ConfigureAppConfiguration((ctx, conf) =>
            {
                conf.AddRemoteSource(action);
            });
        }

        public static IHostBuilder ConfigureRemoteSource(this IHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((ctx, conf) =>
            {
                conf.AddRemoteSource();
            });
        }

        /// <summary>
        /// add remote configuration source
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureRemoteSource(this IHostBuilder builder, SAEOptions options)
        {
            return builder.ConfigureAppConfiguration((ctx, conf) =>
            {
                conf.AddRemoteSource(options);
            });
        }
        /// <summary>
        /// add remote configuration source
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureRemoteSource(this IHostBuilder builder, string url)
        {
            return builder.ConfigureAppConfiguration((ctx, conf) =>
            {
                conf.AddRemoteSource(url);
            });
        }
        /// <summary>
        /// add <seealso cref="MicrosoftConfigurationExtensions.DefaultConfigDirectory"/>  directory As a configuration source
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureJsonFileDirectorySource(this IHostBuilder builder)
        {
            return builder.ConfigureJsonFileDirectorySource(null);
        }
        /// <summary>
        /// add <paramref name="path"/>  directory As a configuration source
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="path">json file directory</param>
        /// <returns></returns>
        public static IHostBuilder ConfigureJsonFileDirectorySource(this IHostBuilder builder,string path)
        {
            return builder.ConfigureAppConfiguration((ctx, conf) =>
            {
                conf.AddJsonFileDirectory(path);
            });
        }

    }
}
