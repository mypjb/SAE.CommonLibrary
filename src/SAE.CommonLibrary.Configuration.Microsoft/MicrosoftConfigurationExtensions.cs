using SAE.CommonLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using SAE.CommonLibrary.Extension;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.Configuration
{
    public static class MicrosoftConfigurationExtensions
    {

        public const string DefaultConfigDirectory = "Config";
        internal const string FileSeparator = ".";
        internal const string JsonSuffix = ".json";
        /// <summary>
        /// Add SAE configuration source
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddSAEConfiguration(this IConfigurationBuilder configurationBuilder, SAEOptions options)
        {
            configurationBuilder.Add(new SAEConfigurationSource(options));

            return configurationBuilder;
        }
        /// <summary>
        /// Add SAE configuration source
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddSAEConfiguration(this IConfigurationBuilder configurationBuilder, Action<SAEOptions> action)
        {
            var option = new SAEOptions();

            action.Invoke(option);

            return configurationBuilder.AddSAEConfiguration(option);
        }
        /// <summary>
        /// Add SAE configuration source
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddSAEConfiguration(this IConfigurationBuilder configurationBuilder, string url)
        {
            return configurationBuilder.AddSAEConfiguration(option =>
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
            return configurationBuilder.AddJsonFileDirectory(DefaultConfigDirectory);
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

            var env = configuration.GetValue<string>(HostDefaults.EnvironmentKey);

            path = path.IsNotNullOrWhiteSpace() ? DefaultConfigDirectory : path;

            var paths = Directory.GetFiles(path, $"*{JsonSuffix}", SearchOption.TopDirectoryOnly)
                                 .OrderBy(s => s)
                                 .ToList();

            var files = paths.Select(s => Path.GetFileNameWithoutExtension(s).Split(FileSeparator).First())
                             .Distinct()
                             .ToArray();

            foreach (var file in files)
            {
                var originFile = Path.Combine(path, $"{file}{JsonSuffix}");
                var envFile = Path.Combine(path, $"{file}{FileSeparator}{env}{JsonSuffix}");
                if (File.Exists(originFile))
                {
                    configurationBuilder.AddJsonFile(originFile, true, true);
                }
                if (File.Exists(envFile))
                {
                    configurationBuilder.AddJsonFile(envFile, true, true);
                }
            }

            return configurationBuilder;
        }
    }
}
