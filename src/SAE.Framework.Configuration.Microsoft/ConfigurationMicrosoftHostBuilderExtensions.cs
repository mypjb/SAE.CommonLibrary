﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SAE.Framework.Configuration;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 添加配置源
    /// </summary>
    public static class ConfigurationMicrosoftHostBuilderExtensions
    {
        /// <summary>
        /// 添加sae远程配置源,并从<see cref="Constants.Config.OptionKey"/>获取<see cref="SAEOptions"/>
        /// </summary>
        /// <param name="builder">Host构建器</param>
        /// <param name="action">初始化配置</param>
        /// <returns>Host构建器</returns>
        public static IHostBuilder ConfigureRemoteSource(this IHostBuilder builder, Action<SAEOptions> action)
        {
            return builder.ConfigureAppConfiguration((ctx, conf) =>
            {
                conf.AddRemoteSource(action);
            });
        }

        /// <summary>
        /// 添加sae远程配置源,并从<paramref name="configurationSection"/>获取<see cref="SAEOptions"/>
        /// </summary>
        /// <param name="builder">Host构建器</param>
        /// <param name="configurationSection">配置子节点名称</param>
        /// <param name="action">初始化配置</param>
        /// <returns>Host构建器</returns>
        public static IHostBuilder ConfigureRemoteSource(this IHostBuilder builder, string configurationSection, Action<SAEOptions> action)
        {
            return builder.ConfigureAppConfiguration((ctx, conf) =>
            {
                conf.AddRemoteSource(configurationSection, action);
            });
        }
        /// <summary>
        /// 添加sae远程配置源
        /// </summary>
        /// <param name="builder">Host构建器</param>
        /// <returns>Host构建器</returns>
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
        /// <param name="builder">Host构建器</param>
        /// <param name="options">远程配置</param>
        /// <returns>Host构建器</returns>
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
        /// <param name="builder">Host构建器</param>
        /// <param name="configurationSection">配置节点名称</param>
        /// <returns>Host构建器</returns>
        public static IHostBuilder ConfigureRemoteSource(this IHostBuilder builder, string configurationSection)
        {
            return builder.ConfigureAppConfiguration((ctx, conf) =>
            {
                conf.AddRemoteSource(configurationSection);
            });
        }
        /// <summary>
        /// 使用<see cref="Constants.Config.DefaultRootDirectory"/>文件夹到配置源
        /// </summary>
        /// <param name="builder">Host构建器</param>
        /// <returns>Host构建器</returns>
        public static IHostBuilder ConfigureJsonFileDirectorySource(this IHostBuilder builder)
        {
            return builder.ConfigureJsonFileDirectorySource(null);
        }
        /// <summary>
        /// 添加<paramref name="path"/>路径到配置源
        /// </summary>
        /// <param name="builder">Host构建器</param>
        /// <param name="path">json文件目录</param>
        /// <returns>Host构建器</returns>
        public static IHostBuilder ConfigureJsonFileDirectorySource(this IHostBuilder builder, string path)
        {
            return builder.ConfigureAppConfiguration((ctx, conf) =>
            {
                conf.AddJsonFileDirectory(path);
            });
        }

    }
}
