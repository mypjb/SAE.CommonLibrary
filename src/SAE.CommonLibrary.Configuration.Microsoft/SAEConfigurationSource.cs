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
    /// <inheritdoc/>
    /// </summary>
    public class SAEConfigurationSource : NewtonsoftJsonStreamConfigurationSource
    {
        private readonly SAEOptions options;
        /// <summary>
        /// new a <see cref="SAEConfigurationSource"/>
        /// </summary>
        /// <param name="options"></param>
        public SAEConfigurationSource(SAEOptions options)
        {
            this.options = options;
        }
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SAEConfigurationProvider(options, this);
        }
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class SAEConfigurationProvider : NewtonsoftJsonStreamConfigurationProvider
    {
        private readonly CancellationTokenSource _cancellationToken;
        private readonly SAEOptions _options;
        private Task pollTask;
        private ILogging _logging;
        /// <summary>
        /// new a <see cref="SAEConfigurationProvider"/>
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
        /// load config
        /// </summary>
        protected async Task LoadAsync()
        {
            if (await this.PullAsync())
            {
                var logging = this.GetLogging();
                logging?.Info($"the latest configuration was successfully pulled from remote:'{this._options.Url}'");
                this.Load(this.Source.Stream);
                this.OnReload();
                logging?.Info("reload configuration ");
            }
        }

        /// <summary>
        /// pull remote config
        /// </summary>
        /// <returns></returns>
        protected async Task<bool> PullAsync()
        {
            var logging = this.GetLogging();

            logging?.Debug($"pull '{this._options.Url}' configuration");

            var rep = await this._options.Client.GetAsync(this._options.Url);

            if (rep.StatusCode == System.Net.HttpStatusCode.NotModified)
            {
                logging?.Debug("cnfiguration not modified ");
                return false;
            }

            rep.EnsureSuccessStatusCode();
            logging?.Info("pull from remote to the latest");
            IEnumerable<string> values;

            if (rep.Headers.TryGetValues(this._options.NextRequestHeaderName, out values))
            {
                this._options.Url = values.First();
                logging?.Info($"set next version pull url '{this._options.Url}'");
            }
            else
            {
                this._options.Url = values.First();
                logging?.Warn($"the next request address was not obtained from the response '{this._options.NextRequestHeaderName}'");
            }

            logging?.Info($"set source stream");

            if (!this._options.ConfiguraionSection.IsNullOrWhiteSpace())
            {
                var json = await rep.Content.ReadAsStringAsync();

                var sections = this._options.ConfiguraionSection
                                            .Split(Constant.ConfiguraionSectionSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                            .ToArray();

                this._logging?.Info($"wrap the data:'{this._options.ConfiguraionSection}'");

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
                    jsonBuilder.Append("}");
                }

            }
            else
            {
                this.Source.Stream = await rep.Content.ReadAsStreamAsync();
            }

            logging?.Info($"persistence to local '{this._options.FileName}'");
            using (var fileStream = new FileStream(this._options.FileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                this.Source.Stream.Position = 0;
                await this.Source.Stream.CopyToAsync(fileStream);
                this.Source.Stream.Position = 0;
            }

            return true;

        }
        /// <summary>
        /// load local config file
        /// </summary>
        /// <returns></returns>
        protected async Task LoadFileAsync(Exception exception = null)
        {
            if (!File.Exists(this._options.FileName))
            {
                throw new FileNotFoundException($"unable to '{this._options.Url}' pull file from remote,And local config file not exist '{this._options.FileName}'", exception);
            }
            var stream = new MemoryStream();
            using (var fs = new FileStream(this._options.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await fs.CopyToAsync(stream);
                stream.Position = 0;
            }

            this.Source.Stream = stream;
        }

        /// <summary>
        /// initial cofnig
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
        /// wait for reload
        /// </summary>
        /// <returns></returns>
        protected virtual async Task WaitForReload()
        {
            await Task.Delay(this._options.PollInterval * 1000, _cancellationToken.Token);
        }

        /// <summary>
        /// poll for secret changes
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
