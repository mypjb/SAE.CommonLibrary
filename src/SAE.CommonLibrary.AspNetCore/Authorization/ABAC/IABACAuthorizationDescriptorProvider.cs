using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    /// <summary>
    /// <see cref="IABACAuthorizationDescriptor"/>提供者
    /// </summary>
    public interface IABACAuthorizationDescriptorProvider
    {
        /// <summary>
        /// 获得<see cref="IABACAuthorizationDescriptor"/>
        /// </summary>
        Task<IABACAuthorizationDescriptor> GetAsync();
    }
}