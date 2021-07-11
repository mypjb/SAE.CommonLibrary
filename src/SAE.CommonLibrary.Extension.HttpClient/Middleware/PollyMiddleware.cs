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

namespace SAE.CommonLibrary.Extension.Middleware
{
    public class PollyMiddleware : DelegatingHandler
    {
        private readonly AsyncRetryPolicy<HttpResponseMessage> _policy;

        public PollyMiddleware(int retryCount, IEnumerable<HttpStatusCode> httpStatusCodes)
        {
            retryCount = retryCount < 0 ? 10 : retryCount;
            var defaultStatusCodes = new[] {
               HttpStatusCode.RequestTimeout, // 408
               HttpStatusCode.InternalServerError, // 500
               HttpStatusCode.BadGateway, // 502
               HttpStatusCode.ServiceUnavailable, // 503
               HttpStatusCode.GatewayTimeout // 504
            };
            httpStatusCodes = httpStatusCodes ?? defaultStatusCodes;

            this._policy = Policy.Handle<HttpRequestException>()
                              .OrResult<HttpResponseMessage>(
                                   r => httpStatusCodes.Contains(r.StatusCode))
                              .RetryAsync(retryCount);
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await this._policy.ExecuteAsync(() => base.SendAsync(request, cancellationToken));
        }
    }
}
