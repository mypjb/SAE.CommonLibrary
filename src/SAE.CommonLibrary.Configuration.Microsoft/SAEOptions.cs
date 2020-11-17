using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SAE.CommonLibrary.Configuration
{
    public class SAEOptions
    {
        public string Url { get; set; }
        public TimeSpan? PollInterval { get; set; }
        public HttpClient Client { get; set; }
    }
}
