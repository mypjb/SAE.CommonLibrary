using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 授权上下文
    /// </summary>
    public class AuthorizeContext
    {
        /// <summary>
        /// 用户信息（请求信息）
        /// </summary>
        /// <value></value>
        public ClaimsPrincipal Principal { get; set; }
    }
}