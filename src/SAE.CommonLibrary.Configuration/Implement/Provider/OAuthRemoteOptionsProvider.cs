using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.Configuration.Implement.Provider
{
    public class OAuthRemoteOptions: RemoteOptions
    {
        
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        protected override void OnChangeClient(HttpClient httpClient)
        {
            base.OnChangeClient(httpClient);
        }

    }
    public class OAuthRemoteOptionsProvider
    {
    }
}
