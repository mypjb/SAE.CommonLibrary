using SAE.CommonLibrary.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    /// <summary>
    /// 端点提供者
    /// </summary>
    public interface IBitmapEndpointProvider
    {
        /// <summary>
        /// 返回端点对象集合
        /// </summary>
        Task<IEnumerable<BitmapEndpoint>> ListAsync();
    }
}
