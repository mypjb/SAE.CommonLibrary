using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAE.Framework.Extension.Middleware
{
    /// <summary>
    /// 重试中间件
    /// </summary>
    /// <remarks>
    /// 在特定的请求异常时，会重试指定次数
    /// </remarks>
    public class PollyMiddleware : DelegatingHandler
    {
        private readonly AsyncRetryPolicy<HttpResponseMessage> _policy;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="retryCount">重试次数</param>
        /// <param name="httpStatusCodes">异常状态码,默认为401、408、500、502、503、504</param>
        public PollyMiddleware(int retryCount, IEnumerable<HttpStatusCode> httpStatusCodes)
        {
            retryCount = retryCount < 0 ? 10 : retryCount;
            var defaultStatusCodes = new[] {
               HttpStatusCode.RequestTimeout, // 408
               HttpStatusCode.InternalServerError, // 500
               HttpStatusCode.BadGateway, // 502
               HttpStatusCode.ServiceUnavailable, // 503
               HttpStatusCode.GatewayTimeout, // 504
               HttpStatusCode.Unauthorized//401
            };
            httpStatusCodes = httpStatusCodes ?? defaultStatusCodes;

            this._policy = Policy.Handle<HttpRequestException>()
                              .OrResult<HttpResponseMessage>(
                                   r =>
                                   {
                                       if (r.StatusCode == HttpStatusCode.Unauthorized &&
                                           r.RequestMessage.Headers.Authorization != null)
                                       {
                                           r.RequestMessage.Headers.Authorization = null;
                                       }
                                       return httpStatusCodes.Contains(r.StatusCode);
                                   })
                              .RetryAsync(retryCount);
        }
        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await this._policy.ExecuteAsync(() => base.SendAsync(request, cancellationToken));
        }
    }
}
