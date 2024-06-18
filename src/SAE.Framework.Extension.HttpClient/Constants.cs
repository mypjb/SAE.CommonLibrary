using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.Extension
{
    /// <summary>
    /// HttpClient Extension常量
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// 到期占比
        /// </summary>
        public const int Expires = 70;
        /// <summary>
        /// 超时时间
        /// </summary>
        public const int Timeout = 1000 * 30;
        /// <summary>
        /// 授权请求超时时间
        /// </summary>
        public const int OAuthTimeout = 1000 * 5;
        /// <summary>
        /// Token无效时是否进行重试
        /// </summary>
        public const bool OAuthManageTokenInvalid = true;
        /// <summary>
        /// Scope
        /// </summary>
        public const string Scope = "api";
        /// <summary>
        /// HttpMessageInvoker Handler 自定名称
        /// </summary>
        public const string HttpMessageInvokerHandler = "_handler";
    }
}
