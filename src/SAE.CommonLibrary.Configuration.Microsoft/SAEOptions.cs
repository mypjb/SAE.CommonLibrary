﻿using System;
using System.Net.Http;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Extension.Middleware;

namespace SAE.CommonLibrary.Configuration
{
    public class SAEOptions
    {
        public SAEOptions()
        {
            this.Client = new HttpClient();
            this.Client.Timeout = TimeSpan.FromMilliseconds(Constants.DefaultClientTimeout);
            this.PollInterval = Constants.DefaultPollInterval;
            //this.SystemKey = Constants.Config.SystemKey;
        }

        //private string systemKey;
        //public string SystemKey
        //{
        //    get => this.systemKey;
        //    set
        //    {
        //        if (!value.IsNullOrWhiteSpace())
        //        {
        //            this.systemKey = value;
        //        }
        //    }
        //}
        public string FileName { get; set; }
        public string Url { get; set; }
        public int PollInterval { get; set; }

        public OAuthOptions OAuth { get; set; }

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

        internal void Check()
        {
            Assert.Build(this.Url)
                  .NotNullOrWhiteSpace();
        }
    }
}
