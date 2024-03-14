using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SAE.CommonLibrary.Extension.Middleware
{
    /// <summary>
    /// 授权配置
    /// </summary>
    public class OAuthOptions
    {
        /// <summary>
        /// ctor
        /// </summary>
        public OAuthOptions()
        {
            this.Expires = Constants.Expires;
            this.Scope = Constants.Scope;
            this.ManageTokenInvalid = Constants.OAuthManageTokenInvalid;
            this.Client = new HttpClient();
            this.Client.Timeout = TimeSpan.FromMilliseconds(Constants.OAuthTimeout);
        }
        /// <summary>
        /// 配置节名称
        /// </summary>
        public const string Option = "oauth";
        /// <summary>
        /// 授权地址
        /// </summary>
        public string Authority { get; set; }
        /// <summary>
        /// 授权域
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        /// 应用id
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 应用Secret
        /// </summary>
        public string AppSecret { get; set; }
        private int expires;
        /// <summary>
        /// 过期百分比默认超过70时间就重置
        /// </summary>
        public int Expires
        {
            get => this.expires;
            set
            {
                if (value > 0 && value < 100)
                {
                    this.expires = value;
                }
            }
        }
        private HttpClient client;
        /// <summary>
        /// 发送授权的HttpClient
        /// </summary>
        public HttpClient Client
        {
            get => client; set
            {
                if (value == null) return;
                client = value;
            }
        }
        /// <summary>
        /// Token无效时是否进行重试
        /// </summary>
        /// <remarks>
        /// 默认true,进行重试。
        /// </remarks>
        public bool ManageTokenInvalid
        {
            get; set;
        }

        /// <summary>
        /// 检查是否正确设置了配置
        /// </summary>
        /// <param name="error">true：错误时触发异常</param>
        /// <returns></returns>
        public bool Check(bool error = false)
        {
            var result = !(this.AppId.IsNullOrWhiteSpace() ||
                           this.AppSecret.IsNullOrWhiteSpace() ||
                           this.Authority.IsNullOrWhiteSpace());

            if (error&&!result)
            {
                throw new SAEException("oauth must offer 'Authority' 'AppId' 'AppSecret'");
            }

            return result;
        }
    }
}
