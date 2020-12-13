using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.NewtonsoftJson;
using SAE.CommonLibrary.Extension;
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
        private readonly HttpClient _client;
        private readonly CancellationTokenSource _cancellationToken;
        private readonly TimeSpan? _pollInterval;
        private string url;
        private Task pollTask;
        private readonly bool _load;

        public SAEConfigurationProvider(SAEOptions options, NewtonsoftJsonStreamConfigurationSource source) : base(source)
        {
            this._client = options.Client;
            this._cancellationToken = new CancellationTokenSource();
            this._pollInterval = options.PollInterval;
            this.url = options.Url;
            this.LoadAsync().GetAwaiter().GetResult();
            this._load = true;
        }

        protected async Task LoadAsync()
        {
            var rep = await this._client.GetAsync(this.url);

            if (rep.StatusCode == System.Net.HttpStatusCode.NotModified)
            {
                return;
            }

            rep.EnsureSuccessStatusCode();

            IEnumerable<string> values;

            if (rep.Headers.TryGetValues(ConfigUrl, out values))
            {
                this.url = values.First();
            }

            this.Source.Stream = await rep.Content.ReadAsStreamAsync();

            if (this._load)
            {
                this.Load(this.Source.Stream);

                this.OnReload();
            }
            
            if (this.pollTask == null && this._pollInterval.HasValue)
            {
                this.pollTask = this.PollForSecretChangesAsync();
            }
        }

        protected virtual async Task WaitForReload()
        {
            await Task.Delay(this._pollInterval.Value, _cancellationToken.Token);
        }

        private async Task PollForSecretChangesAsync()
        {
            while (!this._cancellationToken.IsCancellationRequested)
            {
                await WaitForReload();
                try
                {
                    await LoadAsync();
                }
                catch (Exception)
                {
                    // Ignore
                }
            }
        }
    }
}
