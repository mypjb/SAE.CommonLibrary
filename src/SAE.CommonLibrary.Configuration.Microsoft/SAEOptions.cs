using System;
using System.Net.Http;

namespace SAE.CommonLibrary.Configuration
{
    public class SAEOptions
    {
        public SAEOptions()
        {
            this.Client = new HttpClient();
        }
        public string Url { get; set; }
        public TimeSpan? PollInterval { get; set; }
        private HttpClient client;
        public HttpClient Client
        {
            get => this.client;
            set
            {
                if (value == null) return;

                this.client = value;
            }
        }
    }
}
