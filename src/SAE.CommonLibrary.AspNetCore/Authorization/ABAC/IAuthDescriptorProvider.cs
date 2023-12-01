using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    public interface IAuthDescriptorProvider
    {
        /// <summary>
        /// 获得授权描述符
        /// </summary>
        Task<AuthDescriptor> GetAsync();
    }
}