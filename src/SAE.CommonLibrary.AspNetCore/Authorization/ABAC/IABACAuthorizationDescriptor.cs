using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    /// <summary>
    /// ABAC授权描述
    /// </summary>
    public interface IABACAuthorizationDescriptor
    {
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="ctx"></param>
        Task<bool> AuthAsync(ABACAuthorizationContext ctx);
    }
}