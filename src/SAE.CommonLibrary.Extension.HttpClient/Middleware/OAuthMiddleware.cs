using IdentityModel.Client;
using SAE.CommonLibrary.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Extension.Middleware
{
    /// <summary>
    /// 授权中间件
    /// </summary>
    public class OAuthMiddleware : DelegatingHandler
    {
        /// <summary>
        /// 锁对象
        /// </summary>
        protected readonly object _lock = new object();
        private readonly OAuthOptions _options;
        private readonly Func<ILogging> _loggingRecord;
        /// <summary>
        /// request token
        /// </summary>
        protected RequestToken Token
        {
            get;
            private set;
        }
        /// <summary>
        /// ctor
        /// </summary>
        public OAuthMiddleware(OAuthOptions options)
        {
            this._loggingRecord = HttpClientExtension.LoggerRecord;
            this._options = options;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await RequestTokenAsync(request);

            var responseMessage = await base.SendAsync(request, cancellationToken);

            if (this._options.ManageTokenInvalid &&
                responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                this.Token = null;
                await RequestTokenAsync(request);

                responseMessage = await base.SendAsync(request, cancellationToken);
            }

            return responseMessage;
        }

        private async Task RequestTokenAsync(HttpRequestMessage request)
        {
            if (this.Token == null || !this.Token.Check())
            {
                this._loggingRecord?.Invoke()?.Info("Token 不存在或已过期，重新发送申请请求");
                var tokenClient = this._options.Client;

                var disco = await tokenClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = this._options.Authority,
                    Policy = new DiscoveryPolicy
                    {
                        RequireHttps = false
                    }
                });

                if (disco.IsError)
                {
                    this._loggingRecord?.Invoke()?.Error($"获取DiscoveryDocument失败，{disco.Exception?.Message ?? disco.Error}", disco.Exception);
                    throw disco.Exception ?? new SAEException(StatusCodes.RequestInvalid, disco.Raw); ;
                }

                lock (this._lock)
                {
                    if (this.Token == null || !this.Token.Check())
                    {
                        // request token
                        var tokenResponse = tokenClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                        {
                            Address = disco.TokenEndpoint,
                            ClientId = this._options.AppId,
                            ClientSecret = this._options.AppSecret,
                            Scope = this._options.Scope
                        }).GetAwaiter().GetResult();

                        if (tokenResponse.IsError)
                        {
                            this._loggingRecord?.Invoke()?.Error($"获取Token失败，{tokenResponse.Exception?.Message ?? tokenResponse.Error}", tokenResponse.Exception);
                            throw tokenResponse.Exception ?? new SAEException(StatusCodes.RequestInvalid, tokenResponse.Raw);
                        }
                        this._loggingRecord?.Invoke()?.Info($"Token获取成功,Token:{tokenResponse.AccessToken},ExpiresIn:{tokenResponse.ExpiresIn}");
                        this.Token = new RequestToken(tokenResponse.AccessToken, (int)(tokenResponse.ExpiresIn * 1.0 * this._options.Expires / 100));
                    }
                }
            }

            request.SetBearerToken(this.Token?.AccessToken);
        }
        /// <summary>
        /// 请求令牌
        /// </summary>
        protected class RequestToken
        {
            /// <summary>
            /// 请求令牌
            /// </summary>
            /// <param name="token">令牌</param>
            /// <param name="expiresIn">截止时间</param>
            public RequestToken(string token, int expiresIn)
            {
                this.AccessToken = token;
                this.Deadline = DateTime.Now.AddSeconds(expiresIn);
            }
            /// <summary>
            /// token
            /// </summary>
            public string AccessToken { get; }
            /// <summary>
            /// 截止时间
            /// </summary>
            public DateTime Deadline { get; }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool Check()
            {
                return this.Deadline > DateTime.Now;
            }
        }

    }


}
