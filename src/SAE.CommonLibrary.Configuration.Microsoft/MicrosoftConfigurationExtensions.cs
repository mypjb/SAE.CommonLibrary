using Microsoft.Extensions.Hosting;
using SAE.CommonLibrary;
using SAE.CommonLibrary.Configuration;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Constants = SAE.CommonLibrary.Configuration.Constants;

namespace Microsoft.Extensions.Configuration
{
    public static class MicrosoftConfigurationExtensions
    {
        /// <summary>
        ///  Add remote configuration source
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddRemoteSource(this IConfigurationBuilder configurationBuilder)
        {
            return configurationBuilder.AddRemoteSource(_ => { });
        }

        /// <summary>
        /// Add remote configuration source
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
        /// Add remote configuration source
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddRemoteSource(this IConfigurationBuilder configurationBuilder, Action<SAEOptions> action)
        {
            var configuration = configurationBuilder.Build();

            var section = configuration.GetSection(Constants.Config.OptionKey);

            SAEOptions option;

            if (section.Exists())
            {
                option = section.Get<SAEOptions>();
            }
            else
            {
                option = new SAEOptions();
            }
            if (option.FileName.IsNullOrWhiteSpace())
            {
                var applicationName = configuration.GetSection(HostDefaults.ApplicationKey).Value;

                var env = configuration.GetSection(HostDefaults.EnvironmentKey).Value;

                var root = configuration.GetSection(Constants.Config.RootDirectoryKey)?.Value;

                root = root.IsNullOrWhiteSpace() ? Constants.Config.DefaultRootDirectory : root;

                if (env.IsNullOrWhiteSpace())
                {
                    option.FileName = Path.Combine(root, $"{applicationName}{Constants.JsonSuffix}");
                }
                else
                {
                    option.FileName = Path.Combine(root, $"{applicationName}.{env}{Constants.JsonSuffix}");
                }
                
            }
            //setting oauth
            if (option.OAuth != null && option.OAuth.Check())
            {
                option.Client = option.Client.UseOAuth(option.OAuth);
            }

            action.Invoke(option);
            option.Check();

            if (!configuration.GetSection(Constants.Config.RootDirectoryKey).Exists())
            {
                configurationBuilder.AddInMemoryCollection(new Dictionary<string, string> { { Constants.Config.RootDirectoryKey, Path.GetDirectoryName(option.FileName) } });
            }

            return configurationBuilder.Add(new SAEConfigurationSource(option));
        }
        /// <summary>
        /// Add remote configuration source
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddRemoteSource(this IConfigurationBuilder configurationBuilder, string url)
        {
            return configurationBuilder.AddRemoteSource(option =>
            {
                option.Url = url;
            });
        }

        /// <summary>
        /// Scan <seealso cref="DefaultConfigDirectory"/> all <seealso cref="JsonSuffix"/> file
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddJsonFileDirectory(this IConfigurationBuilder configurationBuilder)
        {
            return configurationBuilder.AddJsonFileDirectory(null);
        }
        /// <summary>
        /// Scan <paramref name="path"/> all <seealso cref="JsonSuffix"/> file
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="path">json file directory</param>
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
                    throw new SAEException($"Not exist directory '{path}' '{applicationDirectory}'.There is at least one of them");
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
