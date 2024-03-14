using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Extension.Middleware
{
    /// <summary>
    /// 错误中间件
    /// </summary>
    /// <remarks>
    /// 在调用发生错误时，会拦截请求。
    /// </remarks>
    public class ExceptionMiddleware : DelegatingHandler
    {
        private readonly Func<HttpResponseMessage, Task> _handler;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="handler">请求异常时，会执行执行该委托</param>
        public ExceptionMiddleware(Func<HttpResponseMessage, Task> handler)
        {
            this._handler = handler;
        }
        /// <inheritdoc/>
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var responseMessage = await base.SendAsync(request, cancellationToken); ;

            if (!responseMessage.IsSuccessStatusCode)
            {
                await this._handler.Invoke(responseMessage);
            }
            return responseMessage;
        }
    }
}

