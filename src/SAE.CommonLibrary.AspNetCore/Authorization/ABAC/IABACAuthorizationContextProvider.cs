using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    /// <summary>
    /// ABAC 上下文提供者
    /// </summary>
    public interface IABACAuthorizationContextProvider
    {
        /// <summary>
        /// 获得<see cref="ABACAuthorizationContext"/>
        /// </summary>
        Task<ABACAuthorizationContext> GetAsync();
    }
}