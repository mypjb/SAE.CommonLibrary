using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Configuration;
using System;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 添加配置源
    /// </summary>
    public static class ConfigurationMicrosoftHostBuilderExtensions
    {
        /// <summary>
        /// 添加sae远程配置源
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
        /// <summary>
        /// 添加sae远程配置源
        /// </summary>
        /// <param name="builder"></param>
        public static IHostBuilder ConfigureRemoteSource(this IHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((ctx, conf) =>
            {
                conf.AddRemoteSource();
            });
        }

        /// <summary>
        /// 添加sae远程配置源
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
        /// 添加sae远程配置源
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
        /// 使用<see cref="MicrosoftConfigurationExtensions.DefaultConfigDirectory"/>文件夹到配置源
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureJsonFileDirectorySource(this IHostBuilder builder)
        {
            return builder.ConfigureJsonFileDirectorySource(null);
        }
        /// <summary>
        /// 添加<paramref name="path"/>路径到配置源
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="path">json文件目录</param>
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
