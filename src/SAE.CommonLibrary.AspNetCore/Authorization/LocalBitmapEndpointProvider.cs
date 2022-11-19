using SAE.CommonLibrary.AspNetCore.Routing;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    /// <summary>
    /// 端点本地存储器
    /// </summary>
    public class LocalBitmapEndpointProvider : AbstractBitmapEndpointProvider, IBitmapEndpointProvider
    {
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="logging">日志</param>
        /// <param name="provider">提供器</param>

        public LocalBitmapEndpointProvider(ILogging<AbstractBitmapEndpointProvider> logging, IPathDescriptorProvider provider) : base(logging)
        {
            this.PathDescriptors = provider.GetDescriptors();
        }
    }
}
