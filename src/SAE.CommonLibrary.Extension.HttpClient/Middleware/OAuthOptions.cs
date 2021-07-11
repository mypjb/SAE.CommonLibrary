using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Extension.Middleware
{
    public class OAuthOptions
    {
        public OAuthOptions()
        {
            this.Expires = Constants.Expires;
            this.Scope = Constants.Scope;
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
        /// 应用key
        /// </summary>
        public string AppKey { get; set; }
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
    }
}
