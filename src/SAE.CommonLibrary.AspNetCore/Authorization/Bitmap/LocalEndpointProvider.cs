using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAE.CommonLibrary.AspNetCore.Routing;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.AspNetCore.Authorization.Bitmap
{
    /// <summary>
    /// 端点本地存储器
    /// </summary>
    public class LocalEndpointProvider : AbstractEndpointProvider, IEndpointProvider
    {
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="logging">日志</param>
        /// <param name="provider">提供器</param>

        public LocalEndpointProvider(ILogging<LocalEndpointProvider> logging, IPathDescriptorProvider provider) : base(logging)
        {
            this.Endpoints = provider.GetDescriptors().Select((desc, index) =>
            {
                return new Endpoint
                {
                    Name = desc.Name,
                    Index = desc.Index,
                    Path = desc.Path,
                    Method = desc.Method
                };
            });
        }
    }
}
