using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SAE.CommonLibrary.Extension.Middleware
{
    public class OAuthOptions
    {
        public OAuthOptions()
        {
            this.Expires = Constants.Expires;
            this.Scope = Constants.Scope;
            this.ManageTokenInvalid = Constants.OAuthManageTokenInvalid;
            this.Client = new HttpClient();
            this.Client.Timeout =  TimeSpan.FromMilliseconds(Constants.OAuthTimeout);
        }
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
        public HttpClient Client
        {
            get => client; set
            {
                if (value == null) return;
                client = value;
            }
        }

        public bool ManageTokenInvalid
        {
            get;set;
        }

        /// <summary>
        /// check oauth whether effective
        /// </summary>
        /// <param name="error">true tagger error </param>
        /// <returns></returns>
        public bool Check(bool error = false)
        {
            if (error)
            {
                Assert.Build(this.AppId.IsNullOrWhiteSpace() ||
                             this.AppSecret.IsNullOrWhiteSpace() ||
                             this.Authority.IsNullOrWhiteSpace())
                      .False("oauth must offer 'Authority' 'AppId' 'AppSecret'");
            }

            return !(this.AppId.IsNullOrWhiteSpace() ||
               this.AppSecret.IsNullOrWhiteSpace() ||
               this.Authority.IsNullOrWhiteSpace());
        }
    }
}
