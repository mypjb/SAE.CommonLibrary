using System;
using System.Net.Http;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.Configuration
{
    public class SAEOptions
    {
        public SAEOptions()
        {
            this.Client = new HttpClient();
            this.PollInterval = Constants.DefaultPollInterval;
        }

        public string FileName { get; set; }
        public string Url { get; set; }
        public int PollInterval { get; set; }
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
