using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using SAE.Framework.Extension;
using SAE.Framework.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAE.Framework.Configuration
{
    /// <summary>
    /// SAE配置源
    /// </summary>
    public class SAEConfigurationSource : JsonStreamConfigurationSource
    {
        private readonly SAEOptions options;
        private IConfigurationProvider provider;
        /// <summary>
        /// 创建一额新的 <see cref="SAEConfigurationSource"/>
        /// </summary>
        /// <param name="options">源配置</param>
        public SAEConfigurationSource(SAEOptions options)
        {
            this.options = options;
        }
        /// <summary>
        /// 构造配置提供程序
        /// </summary>
        /// <param name="builder">配置构建器</param>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            this.provider ??= new SAEConfigurationProvider(options, this);
            this.provider.Load();

            var nodeName = this.options.IncludeEndpointConfiguration;

            if (!nodeName.IsNullOrWhiteSpace())
            {
                if (!this.options.ConfigurationSection.IsNullOrWhiteSpace())
                {
                    nodeName = $"{this.options.ConfigurationSection}{Constants.ConfigSeparator}{nodeName}";
                }

                var indexChildKeys = this.provider.GetChildKeys(Enumerable.Empty<string>(), nodeName);

                if (indexChildKeys != null && indexChildKeys.Any())
                {
                    indexChildKeys = indexChildKeys.Distinct().ToArray();
                    var configurationBuilder = new ConfigurationBuilder();

                    configurationBuilder.AddConfiguration(new ConfigurationRoot(new[] { this.provider }));
                    foreach (var indexChildKey in indexChildKeys)
                    {
                        var key = $"{nodeName}{Constants.ConfigSeparator}{indexChildKey}{Constants.ConfigSeparator}";

                        if (this.provider.TryGet($"{key}{Constants.Config.Include.Name}", out var includeName) &&
                            this.provider.TryGet($"{key}{Constants.Config.Include.Url}", out var includeUrl))
                        {

                            var childOption = new SAEOptions(this.options);

                            var rootFileName = Path.GetFileNameWithoutExtension(this.options.FullPath);

                            var rootFileIndex = rootFileName.LastIndexOf('.');

                            if (rootFileIndex != -1 && rootFileIndex < rootFileName.Length)
                            {
                                includeName = $"{includeName}{rootFileName.Substring(rootFileIndex)}";
                            }

                            childOption.FullPath = Path.Combine(Path.GetDirectoryName(this.options.FullPath), $"{includeName}{Path.GetExtension(this.options.FullPath)}");

                            childOption.FileName = Path.GetFileName(childOption.FullPath);
                            childOption.Url = includeUrl;

                            if (this.provider.TryGet($"{key}{Constants.Config.Include.NodeName}", out var includeNodeName))
                            {
                                childOption.ConfigurationSection = includeNodeName;
                            }

                            if (childOption.FileName.Equals(options.FileName))
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"子集配置的文件名称({childOption.FileName})与父级({this.options.FileName})冲突，请更改后重新启动系统进行加载！");
                                Console.ResetColor();
                                continue;
                            }
                            configurationBuilder.Add(new SAEConfigurationSource(childOption));
                        }
                    }
                    var source = new ChainedConfigurationSource()
                    {
                        Configuration = configurationBuilder.Build()
                    };
                    return source.Build(configurationBuilder);
                }
            }

            return this.provider;

        }
    }

    /// <summary>
    /// SAE 配置提供者
    /// </summary>
    public class SAEConfigurationProvider : JsonStreamConfigurationProvider
    {
        private readonly CancellationTokenSource _cancellationToken;
        private readonly SAEOptions _options;
        private Task pollTask;
        private ILogging Logging
        {
            get
            {
                return (ILogging)ServiceFacade.GetService<ILogging<SAEConfigurationProvider>>() ?? new EmptyLogging();
            }
        }
        /// <summary>
        /// 创建一个新的<see cref="SAEConfigurationProvider"/>
        /// </summary>
        /// <param name="options">源配置</param>
        /// <param name="source">json配置源</param>
        public SAEConfigurationProvider(SAEOptions options, JsonStreamConfigurationSource source) : base(source)
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
                this.Logging.Info($"成功从远程拉取最新配置:'{this._options.Url}'");
                this.Load(this.Source.Stream);
                this.OnReload();
                this.Logging.Info("重新加载配置");
            }
        }
        
        /// <inheritdoc/>
        public override void Load()
        {
            if (this.Source.Stream == null || !this.Source.Stream.CanRead)
            {
                this.Logging.Warn("流处于不可读状态。");
                return;
            }
            if (this.Source.Stream.Position > 0)
            {
                this.Source.Stream.Position = 0;
            }
            this.Load(this.Source.Stream);
        }

        /// <summary>
        /// 拉取远程配置
        /// </summary>
        /// <returns></returns>
        protected async Task<bool> PullAsync()
        {

            this.Logging.Debug($"从'{this._options.Url}'拉取配置");

            using (var rep = await this._options.Client.GetAsync(this._options.Url))
            {
                if (rep.StatusCode == System.Net.HttpStatusCode.NotModified)
                {
                    this.Logging.Debug("配置尚未更改，跳过后续步骤");
                    return false;
                }

                rep.EnsureSuccessStatusCode();
                this.Logging.Info("从远程拉取到最新配置");
                IEnumerable<string> values;

                if (rep.Headers.TryGetValues(this._options.NextRequestHeaderName, out values))
                {
                    this._options.Url = values.First();
                    this.Logging.Info($"设置下一次拉取请求为'{this._options.Url}'");
                }
                else
                {
                    this.Logging.Warn($"没有从响应中获得下一次请求地址'{this._options.NextRequestHeaderName}'");
                }

                this.Logging.Info($"设置配置源二进制流");

                if (!this._options.ConfigurationSection.IsNullOrWhiteSpace())
                {
                    var json = await rep.Content.ReadAsStringAsync();

                    var sections = this._options.ConfigurationSection
                                                .Split(Constants.ConfigurationSectionSeparator,
                                                       StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                                .ToArray();

                    this.Logging.Info($"包装配置:'{this._options.ConfigurationSection}'");

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

                    this.Logging.Debug($"拉取到的json数据：\r\n{jsonData}");

                    await this.SetSourceAsync(new MemoryStream(SAE.Framework.Constants.Encoding.GetBytes(jsonData)));
                }
                else
                {
                    await this.SetSourceAsync(new MemoryStream(await rep.Content.ReadAsByteArrayAsync()));
                }

                this.Logging.Info($"持久化配置到'{this._options.FullPath}'");

                var configDirectory = Path.GetDirectoryName(this._options.FullPath);

                if (!Directory.Exists(configDirectory))
                {
                    this.Logging.Warn($"配置目录不存在'{configDirectory}',自动创建它");
                    Directory.CreateDirectory(configDirectory);
                }

                using (var fileStream = new FileStream(this._options.FullPath, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    this.Source.Stream.Position = 0;
                    await this.Source.Stream.CopyToAsync(fileStream);
                    this.Source.Stream.Position = 0;
                }
            }
            return true;

        }
        /// <summary>
        /// 从本地文件当中获得配置
        /// </summary>
        /// <param name="exception">异常</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">远程拉取失败，且<see cref="SAEOptions.FullPath"/>不存在，将会触发。</exception>
        protected async Task LoadFileAsync(Exception exception = null)
        {
            var fileName = this._options.FullPath;
            if (!File.Exists(fileName))
            {
                if (string.IsNullOrWhiteSpace(this._options.FullPathBackup) || !File.Exists(this._options.FullPathBackup))
                {
                    throw new FileNotFoundException($"从远程'{this._options.Url}'拉取配置失败,并且无法从本地'{this._options.FullPath}{(string.IsNullOrWhiteSpace(this._options.FullPathBackup) ? string.Empty : $":{this._options.FullPathBackup}")}'获取文件配置", exception);
                }

                fileName = this._options.FullPathBackup;
            }
            var stream = new MemoryStream();
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await fs.CopyToAsync(stream);
                stream.Position = 0;
            }

            await this.SetSourceAsync(stream);
        }

        /// <summary>
        /// 设置源
        /// </summary>
        /// <param name="stream">配置流</param>
        protected async Task SetSourceAsync(Stream stream)
        {
            var oldStream = this.Source.Stream;
            this.Source.Stream = stream;
            try
            {
                if (oldStream != null)
                {
                    this.Logging.Debug("将旧配置流进行释放");
                    await oldStream.DisposeAsync();
                }
            }
            catch (Exception ex)
            {
                this.Logging.Warn("旧配置流行释放失败，这可能该流被提前释放导致的", ex);
            }
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
                    this.Logging.Error($"load configuration error:{ex.Message}", ex);
                    // Ignore
                }
            }
        }
     
    }
}
