using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 授权服务
    /// </summary>
    public interface IAuthorizeService
    {
        /// <summary>
        /// 授权
        /// </summary>
        /// <returns>授权是否成功</returns>
        Task<bool> AuthAsync();

        /// <summary>
        /// 获得授权策略集合
        /// </summary>
        Task<AuthorizationPolicy[]> GetAuthorizationPoliciesAsync();
        /// <summary>
        /// 获得授权描述符
        /// </summary>
        Task<AuthDescriptor> GetAuthDescriptorAsync();
    }
}