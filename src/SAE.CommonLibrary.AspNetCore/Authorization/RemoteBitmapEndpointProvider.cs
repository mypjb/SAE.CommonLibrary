using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SAE.CommonLibrary.AspNetCore.Routing;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    /// <summary>
    /// 远程端点配置
    /// </summary>
    public class RemoteBitmapEndpointOptions
    {
        /// <summary>
        /// 配置节
        /// </summary>
        public const string Option = Constants.BitmapAuthorize.Option;
        /// <summary>
        /// 端点
        /// </summary>
        /// <value></value>
        public string BitmapEndpoint { get; set; }
    }
    /// <summary>
    /// 远程端点提供程序
    /// </summary>
    /// <inheritdoc/>
    public class RemoteBitmapEndpointProvider : IBitmapEndpointProvider
    {
        private readonly RemoteBitmapEndpointOptions _options;
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="options"></param>
        public RemoteBitmapEndpointProvider(RemoteBitmapEndpointOptions options)
        {
            this._options = options;
        }
        public async Task<IEnumerable<BitmapEndpoint>> ListAsync()
        {
            var client = new HttpClient();

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, this._options.BitmapEndpoint);

            var responseMessage = await client.SendAsync(requestMessage);

            return await responseMessage.AsAsync<IEnumerable<BitmapEndpoint>>();
        }
    }
}
