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
        /// 传入端点地址返回端点对象
        /// </summary>
        /// <param name="paths"></param>
        Task<IEnumerable<BitmapEndpoint>> FindsAsync(IEnumerable<IPathDescriptor> paths);
    }
}
