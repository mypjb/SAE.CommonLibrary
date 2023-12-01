using SAE.CommonLibrary.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Authorization.Bitmap
{
    /// <summary>
    /// 端点提供者
    /// </summary>
    public interface IEndpointProvider
    {
        /// <summary>
        /// 返回端点对象集合
        /// </summary>
        Task<IEnumerable<Endpoint>> ListAsync();
    }
}
