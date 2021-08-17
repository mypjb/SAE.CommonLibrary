using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.NewtonsoftJson;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Configuration
{
    public class SAEConfigurationSource : NewtonsoftJsonStreamConfigurationSource
    {
        private readonly SAEOptions options;

        public SAEConfigurationSource(SAEOptions options)
        {
            this.options = options;
        }
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SAEConfigurationProvider(options, this);
        }
    }

    public class SAEConfigurationProvider : NewtonsoftJsonStreamConfigurationProvider
    {
        public const string ConfigUrl = "Config-Next";
        private readonly CancellationTokenSource _cancellationToken;
        private readonly SAEOptions _options;
        private Task pollTask;
        private ILogging _logging;

        public SAEConfigurationProvider(SAEOptions options, NewtonsoftJsonStreamConfigurationSource source) : base(source)
        {
            this._cancellationToken = new CancellationTokenSource();
            this._options = options;
            this.Init();
        }

        protected async Task LoadAsync()
        {
            if (await this.PullAsync())
            {
                var logging = this.GetLogging();
                logging?.Info($"The latest configuration was successfully pulled from remote:'{this._options.Url}'");
                this.Load(this.Source.Stream);
                this.OnReload();
                logging?.Info("Reload configuration ");
            }
        }
        
        /// <summary>
        /// Pull remote config
        /// </summary>
        /// <returns></returns>
        protected async Task<bool> PullAsync()
        {
            var logging= this.GetLogging();

            logging?.Debug($"Pull '{this._options.Url}' configuration");

            var rep = await this._options.Client.GetAsync(this._options.Url);

            if (rep.StatusCode == System.Net.HttpStatusCode.NotModified)
            {
                logging?.Debug("Configuration not modified ");
                return false;
            }

            rep.EnsureSuccessStatusCode();
            logging?.Info("Pull from remote to the latest");
            IEnumerable<string> values;

            if (rep.Headers.TryGetValues(ConfigUrl, out values))
            {
                this._options.Url = values.First();
                logging?.Info($"Set next version pull url '{this._options.Url}'");
            }

            logging?.Info($"Set source stream");
            this.Source.Stream = await rep.Content.ReadAsStreamAsync();

            logging?.Info($"Persistence to local '{this._options.FileName}'");
            using (var fileStream = new FileStream(this._options.FileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                this.Source.Stream.Position = 0;
                await this.Source.Stream.CopyToAsync(fileStream);
                this.Source.Stream.Position = 0;
            }

            return true;

        }
        /// <summary>
        /// Load local config file
        /// </summary>
        /// <returns></returns>
        protected async Task LoadFileAsync(Exception exception = null)
        {
            if (!File.Exists(this._options.FileName))
            {
                throw new FileNotFoundException($"Unable to '{this._options.Url}' pull file from remote,And local config file not exist '{this._options.FileName}'", exception);
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
        /// Initial Cofnig
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
        /// Wait for reload
        /// </summary>
        /// <returns></returns>
        protected virtual async Task WaitForReload()
        {
            await Task.Delay(this._options.PollInterval * 1000, _cancellationToken.Token);
        }

        /// <summary>
        /// Poll for secret changes
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
                    this.GetLogging()?.Error($"Load configuration error:{ex.Message}", ex);
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
