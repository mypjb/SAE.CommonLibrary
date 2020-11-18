using Microsoft.Extensions.Configuration;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Configuration
{
    public class SAEConfigurationSource : IConfigurationSource
    {
        private readonly SAEOptions options;

        public SAEConfigurationSource(SAEOptions options)
        {
            this.options = options;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SAEConfigurationProvider(options);
        }
    }


    public class SAEConfigurationProvider : ConfigurationProvider
    {
        public const string ConfigUrl = "Config-Url";
        private readonly HttpClient _client;
        private readonly CancellationTokenSource _cancellationToken;
        private readonly TimeSpan? _pollInterval;
        private string url;
        private Task pollTask;

        public SAEConfigurationProvider(SAEOptions options)
        {
            this._client = options.Client;
            this._cancellationToken = new CancellationTokenSource();
            this._pollInterval = options.PollInterval;
            this.url = options.Url;
        }

        public override void Load()
        {
            this.LoadAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task LoadAsync()
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

            var data = await rep.AsAsync<Dictionary<string, string>>();

            this.SetData(data);

            if (this.pollTask == null && this._pollInterval.HasValue)
            {
                this.pollTask = this.PollForSecretChangesAsync();
            }

        }

        private void SetData(Dictionary<string, string> data)
        {
            this.Data = data;
            if (this.pollTask != null)
            {
                this.OnReload();
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
