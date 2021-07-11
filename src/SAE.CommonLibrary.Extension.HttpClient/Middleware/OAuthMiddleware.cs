using IdentityModel.Client;
using SAE.CommonLibrary.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Extension.Middleware
{
    public class OAuthMiddleware : DelegatingHandler
    {
        protected readonly object _lock = new object();
        private readonly OAuthOptions _options;
        private readonly Func<ILogging> _loggingRecord;
        protected RequestToken Token
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="authority">认证地址</param>
        /// <param name="appId">appid</param>
        /// <param name="appKey">appkey</param>
        public OAuthMiddleware(OAuthOptions options)
        {
            this._loggingRecord = HttpClientExtension.LoggerRecord;
            this._options = options;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await RequestTokenAsync(request);
            return await base.SendAsync(request, cancellationToken);
        }

        private async Task RequestTokenAsync(HttpRequestMessage request)
        {
            if (this.Token == null || !this.Token.Check())
            {
                this._loggingRecord?.Invoke().Info("Token 不存在或已过期，重新发送申请请求");
                var tokenClient = new HttpClient();

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
                    this._loggingRecord?.Invoke()?.Error($"获取DiscoveryDocument失败，{disco.Exception?.Message}", disco.Exception);
                    throw disco.Exception;
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
                            ClientSecret = this._options.AppKey,
                            Scope = this._options.Scope
                        }).GetAwaiter().GetResult();

                        if (tokenResponse.IsError)
                        {
                            this._loggingRecord?.Invoke()?.Error($"获取Token失败，{tokenResponse.Exception?.Message}", tokenResponse.Exception);
                            throw tokenResponse.Exception;
                        }
                        this._loggingRecord?.Invoke()?.Info($"Token获取成功,Token:{tokenResponse.AccessToken},ExpiresIn:{tokenResponse.ExpiresIn}");
                        this.Token = new RequestToken(tokenResponse.AccessToken, tokenResponse.ExpiresIn * (this._options.Expires / 100));
                    }
                }
            }

            request.SetBearerToken(this.Token.AccessToken);
        }
        protected class RequestToken
        {
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
