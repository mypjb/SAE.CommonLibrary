using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SAE.CommonLibrary.AspNetCore.Routing;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    public class RemoteBitmapEndpointOptions
    {
        public const string Option = "authorize";
        public string Url { get; set; }
    }
    public class RemoteBitmapEndpointProvider : IBitmapEndpointProvider
    {
        private readonly RemoteBitmapEndpointOptions _options;

        public RemoteBitmapEndpointProvider(RemoteBitmapEndpointOptions options)
        {
            this._options = options;
        }
        public async Task<IEnumerable<BitmapEndpoint>> FindALLAsync(IEnumerable<IPathDescriptor> descriptors)
        {
            var client = new HttpClient();

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, this._options.Url);

            requestMessage.AddJsonContent(descriptors);

            var responseMessage = await client.SendAsync(requestMessage);

            return await responseMessage.AsAsync<IEnumerable<BitmapEndpoint>>();
        }
    }
}
