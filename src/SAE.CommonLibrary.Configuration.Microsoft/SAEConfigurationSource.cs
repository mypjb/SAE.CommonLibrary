using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.NewtonsoftJson;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.Configuration
{
    /// <summary>
    /// SAE配置源
    /// </summary>
    /// <inheritdoc/>
    public class SAEConfigurationSource : NewtonsoftJsonStreamConfigurationSource
    {
        private readonly SAEOptions options;
        private IConfigurationProvider provider;
        /// <summary>
        /// 创建一额新的 <see cref="SAEConfigurationSource"/>
        /// </summary>
        /// <param name="options"></param>
        public SAEConfigurationSource(SAEOptions options)
        {
            this.options = options;
        }
        /// <summary>
        /// 构造配置提供程序
        /// </summary>
        /// <param name="builder"></param>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            this.provider ??= new SAEConfigurationProvider(options, this);
            this.provider.Load();

            var nodeName = this.options.IncludeEndpointConfiguration;

            if (!this.options.ConfigurationSection.IsNullOrWhiteSpace())
            {
                nodeName = $"{nodeName}{Constants.ConfigSeparator}{this.options.ConfigurationSection}";
            }

            var childKeys = this.provider.GetChildKeys(Enumerable.Empty<string>(), nodeName);

            if (childKeys != null && childKeys.Any())
            {
                var configurationBuilder = new ConfigurationBuilder();
                configurationBuilder.AddConfiguration(new ConfigurationRoot(new[] { this.provider }));
                foreach (var childKey in childKeys)
                {
                    configurationBuilder.Add(new SAEConfigurationSource(new SAEOptions(this.options)));
                }
                var source = new ChainedConfigurationSource()
                {
                    Configuration = configurationBuilder.Build()
                };
                return source.Build(configurationBuilder);
            }
            else
            {
                return this.provider;
            }

        }
    }

    /// <summary>
    /// SAE 配置提供者
    /// </summary>
    /// <inheritdoc/>
    public class SAEConfigurationProvider : NewtonsoftJsonStreamConfigurationProvider
    {
        private readonly CancellationTokenSource _cancellationToken;
        private readonly SAEOptions _options;
        private Task pollTask;
        private ILogging _logging;
        /// <summary>
        /// 创建一个新的<see cref="SAEConfigurationProvider"/>
        /// </summary>
        /// <param name="options"></param>
        /// <param name="source"></param>
        public SAEConfigurationProvider(SAEOptions options, NewtonsoftJsonStreamConfigurationSource source) : base(source)
        {
            this._cancellationToken = new CancellationTokenSource();
            this._options = options;
            this.Init();
        }
        /// <summary>
        /// 加载配置
        /// </summary>
        protected async Task LoadAsync()
        {
            if (await this.PullAsync())
            {
                var logging = this.GetLogging();
                logging?.Info($"成功从远程拉取最新配置:'{this._options.Url}'");
                this.Load(this.Source.Stream);
                this.OnReload();
                logging?.Info("重新加载配置");
            }
        }

        /// <summary>
        /// 拉取远程配置
        /// </summary>
        /// <returns></returns>
        protected async Task<bool> PullAsync()
        {
            var logging = this.GetLogging();

            logging?.Debug($"从'{this._options.Url}'拉取配置");

            var rep = await this._options.Client.GetAsync(this._options.Url);

            if (rep.StatusCode == System.Net.HttpStatusCode.NotModified)
            {
                logging?.Debug("配置尚未更改，跳过后续步骤");
                return false;
            }

            rep.EnsureSuccessStatusCode();
            logging?.Info("从远程拉取到最新配置");
            IEnumerable<string> values;

            if (rep.Headers.TryGetValues(this._options.NextRequestHeaderName, out values))
            {
                this._options.Url = values.First();
                logging?.Info($"设置下一次拉取请求为'{this._options.Url}'");
            }
            else
            {
                logging?.Warn($"没有从响应中获得下一次请求地址'{this._options.NextRequestHeaderName}'");
            }

            logging?.Info($"设置配置源二进制流");

            if (!this._options.ConfigurationSection.IsNullOrWhiteSpace())
            {
                var json = await rep.Content.ReadAsStringAsync();

                var sections = this._options.ConfigurationSection
                                            .Split(Constants.ConfigurationSectionSeparator,
                                                   StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                            .ToArray();

                this._logging?.Info($"包装配置:'{this._options.ConfigurationSection}'");

                var jsonBuilder = new StringBuilder();

                foreach (var section in sections)
                {
                    jsonBuilder.Append("{\"")
                               .Append(section.Trim())
                               .Append("\":");
                }

                jsonBuilder.Append(json);

                foreach (var _ in sections)
                {
                    jsonBuilder.Append('}');
                }

                var jsonData = jsonBuilder.ToString();

                this._logging?.Debug($"拉取到的json数据：\r\n{jsonData}");

                this.Source.Stream = new MemoryStream(SAE.CommonLibrary.Constants.Encoding.GetBytes(jsonData));
            }
            else
            {
                this.Source.Stream = await rep.Content.ReadAsStreamAsync();
            }

            logging?.Info($"持久化配置到'{this._options.FullPath}'");

            var configDirectory = Path.GetDirectoryName(this._options.FullPath);

            if (!Directory.Exists(configDirectory))
            {
                this._logging?.Warn($"配置目录不存在'{configDirectory}',自动创建它");
                Directory.CreateDirectory(configDirectory);
            }

            using (var fileStream = new FileStream(this._options.FullPath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                this.Source.Stream.Position = 0;
                await this.Source.Stream.CopyToAsync(fileStream);
                this.Source.Stream.Position = 0;
            }

            return true;

        }
        /// <summary>
        /// 从本地文件当中获得配置
        /// </summary>
        /// <returns></returns>
        protected async Task LoadFileAsync(Exception exception = null)
        {
            if (!File.Exists(this._options.FullPath))
            {
                throw new FileNotFoundException($"从远程'{this._options.Url}'拉取配置失败,并且无法从本地'{this._options.FullPath}'获取文件配置", exception);
            }
            var stream = new MemoryStream();
            using (var fs = new FileStream(this._options.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await fs.CopyToAsync(stream);
                stream.Position = 0;
            }

            this.Source.Stream = stream;
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        protected void Init()
        {
            try
            {
                this.PullAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                this.LoadFileAsync(ex).GetAwaiter().GetResult();
            }
            finally
            {
                if (this.pollTask == null && this._options.PollInterval > 0)
                {
                    this.pollTask = this.PollForSecretChangesAsync();
                }
            }
        }
        /// <summary>
        /// 等待加载
        /// </summary>
        /// <returns></returns>
        protected virtual async Task WaitForReload()
        {
            await Task.Delay(this._options.PollInterval * 1000, _cancellationToken.Token);
        }

        /// <summary>
        /// 异步轮询
        /// </summary>
        /// <returns></returns>
        private async Task PollForSecretChangesAsync()
        {
            while (!this._cancellationToken.IsCancellationRequested)
            {
                await WaitForReload();
                try
                {
                    await LoadAsync();
                }
                catch (Exception ex)
                {
                    this.GetLogging()?.Error($"load configuration error:{ex.Message}", ex);
                    // Ignore
                }
            }
        }
        /// <summary>
        /// 获得日志记录器
        /// </summary>
        private ILogging GetLogging()
        {
            if (this._logging == null)
            {
                this._logging = ServiceFacade.GetService<ILogging<SAEConfigurationProvider>>();
            }
            return this._logging;
        }
    }
}
