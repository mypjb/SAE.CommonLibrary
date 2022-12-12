using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Hosting;
using SAE.CommonLibrary;
using SAE.CommonLibrary.Configuration;
using SAE.CommonLibrary.Extension;
using Constants = SAE.CommonLibrary.Configuration.Constants;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class MicrosoftConfigurationExtensions
    {
        /// <summary>
        ///  添加SAE远程配置源
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddRemoteSource(this IConfigurationBuilder configurationBuilder)
        {
            return configurationBuilder.AddRemoteSource(_ => { });
        }

        /// <summary>
        /// 添加SAE远程配置源
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddRemoteSource(this IConfigurationBuilder configurationBuilder, SAEOptions options)
        {
            return configurationBuilder.AddRemoteSource(op =>
            {
                op.Extend(options);
            });
        }
        /// <summary>
        /// 添加SAE远程配置源,并从<see cref="Constants.Config.OptionKey"/>获取<see cref="SAEOptions"/>
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="action">初始化配置</param>
        public static IConfigurationBuilder AddRemoteSource(this IConfigurationBuilder configurationBuilder, Action<SAEOptions> action)
        {
            return configurationBuilder.AddRemoteSource(null, action);
        }
        /// <summary>
        /// 添加SAE远程配置源，并从<paramref name="configurationSection"/>获取<see cref="SAEOptions"/>
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="configurationSection">配置子节点名称</param>
        /// <param name="action">初始化配置</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddRemoteSource(this IConfigurationBuilder configurationBuilder, string configurationSection, Action<SAEOptions> action)
        {
            configurationSection = configurationSection.IsNullOrWhiteSpace() ? Constants.Config.OptionKey : configurationSection;

            var configuration = configurationBuilder.Build();

            var section = configuration.GetSection(configurationSection);

            SAEOptions option;

            if (section.Exists())
            {
                Console.WriteLine($"找到'{configurationSection}'配置节!");
                option = section.Get<SAEOptions>();
            }
            else
            {
                Console.WriteLine($"所提供的配置上下文不存在配置节'{configurationSection}'");
                option = new SAEOptions();
            }

            if (option.FullPath.IsNullOrWhiteSpace())
            {
                var applicationName = configuration.GetSection(HostDefaults.ApplicationKey).Value;

                applicationName = applicationName.IsNullOrWhiteSpace() ? Guid.NewGuid().ToString("N") : applicationName;

                var env = configuration.GetSection(HostDefaults.EnvironmentKey).Value;

                var root = configuration.GetSection(Constants.Config.RootDirectoryKey)?.Value;

                root = root.IsNullOrWhiteSpace() ? Constants.Config.DefaultRootDirectory : root;

                if (option.FileName.IsNullOrWhiteSpace())
                {
                    if (env.IsNullOrWhiteSpace())
                    {
                        option.FullPath = Path.Combine(root, $"{applicationName}{Constants.JsonSuffix}");
                    }
                    else
                    {
                        option.FullPath = Path.Combine(root, $"{applicationName}.{env}{Constants.JsonSuffix}");
                    }

                }
                else
                {
                    option.FullPath = Path.Combine(root, option.FileName);
                }
            }

            if (option.FileName.IsNullOrWhiteSpace())
            {
                option.FileName = Path.GetFileName(option.FullPath);
            }


            //setting oauth
            if (option.OAuth != null && option.OAuth.Check())
            {
                option.Client = option.Client.UseOAuth(option.OAuth);
            }

            action?.Invoke(option);

            Console.WriteLine($"远程配置信息：{option.ToJsonString()}");
            option.Check();

            if (!configuration.GetSection(Constants.Config.RootDirectoryKey).Exists())
            {
                Console.WriteLine($"未找到根目录'{Constants.Config.RootDirectoryKey}'配置节，自动设置默认根目录");
                configurationBuilder.AddInMemoryCollection(new Dictionary<string, string> { { Constants.Config.RootDirectoryKey, Path.GetDirectoryName(option.FullPath) } });
            }

            return configurationBuilder.Add(new SAEConfigurationSource(option));
        }
        /// <summary>
        /// 添加SAE远程配置源，并从<paramref name="configurationSection"/>获取<see cref="SAEOptions"/>
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="configurationSection">配置子节点名称</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddRemoteSource(this IConfigurationBuilder configurationBuilder, string configurationSection)
        {
            return configurationBuilder.AddRemoteSource(configurationSection, _ => { });
        }

        /// <summary>
        /// 扫描 <see cref="Constants.Config.DefaultRootDirectory"/> 目录所有 <see cref="Constants.JsonSuffix"/> 后缀的文件
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddJsonFileDirectory(this IConfigurationBuilder configurationBuilder)
        {
            return configurationBuilder.AddJsonFileDirectory(null);
        }
        /// <summary>
        /// 扫描 <paramref name="path"/>目录所有 <see cref="Constants.JsonSuffix"/> 后缀的文件
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="path">json文件目录</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddJsonFileDirectory(this IConfigurationBuilder configurationBuilder, string path)
        {
            var configuration = configurationBuilder.Build();

            var env = configuration.GetSection(HostDefaults.EnvironmentKey).Get<string>() ?? string.Empty;

            var applicationName = configuration.GetSection(HostDefaults.ApplicationKey).Get<string>() ?? string.Empty;

            if (path.IsNullOrWhiteSpace())
            {
                var section = env.IsNullOrWhiteSpace() ? configuration.GetSection(Constants.Config.RootDirectoryKey) :
                                                         configuration.GetSection($"{env}{Constants.ConfigSeparator}{Constants.Config.RootDirectoryKey}");

                path = section.Value.IsNullOrWhiteSpace() ? Constants.Config.DefaultRootDirectory : section.Value;
            }

            var applicationDirectory = Path.Combine(path, applicationName);

            if (Directory.Exists(applicationDirectory))
            {
                path = applicationDirectory;
            }
            else
            {
                if (!Directory.Exists(path))
                {
                    throw new SAEException($"目录 '{path}' '{applicationDirectory}' 至少得有一个存在");
                }
            }

            var paths = Directory.GetFiles(path, $"*{Constants.JsonSuffix}", SearchOption.TopDirectoryOnly)
                                 .OrderBy(s => s)
                                 .ToList();

            var files = paths.Select(s =>
            {
                var fileName = Path.GetFileNameWithoutExtension(s);
                var fileSeparatorIndex = fileName.LastIndexOf(Constants.FileSeparator);
                if (fileSeparatorIndex != -1)
                {
                    fileName = fileName.Substring(0, fileSeparatorIndex);
                }
                return fileName;
            }).Distinct()
              .ToArray();

            foreach (var file in files)
            {
                var originFile = Path.Combine(path, $"{file}{Constants.JsonSuffix}");

                if (File.Exists(originFile))
                {
                    configurationBuilder.AddJsonFile(originFile, true, true);
                }

                if (!env.IsNullOrWhiteSpace())
                {
                    var envFile = Path.Combine(path, $"{file}{Constants.FileSeparator}{env}{Constants.JsonSuffix}");
                    if (File.Exists(envFile))
                    {
                        configurationBuilder.AddJsonFile(envFile, true, true);
                    }
                }
            }

            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string> { { Constants.Config.RootDirectoryKey, path } });

            return configurationBuilder;
        }
    }
}