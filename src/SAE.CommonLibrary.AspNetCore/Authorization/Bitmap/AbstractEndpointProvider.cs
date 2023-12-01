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
    /// <inheritdoc/>
    /// <summary>
    /// 抽象的端点提供程序
    /// </summary>
    public abstract class AbstractEndpointProvider : IEndpointProvider
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected readonly ILogging _logging;
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="logging">日志记录器</param>
        public AbstractEndpointProvider(ILogging<AbstractEndpointProvider> logging)
        {
            this._logging = logging;
            this.Endpoints = Array.Empty<Endpoint>();
        }
        private IEnumerable<Endpoint> endpoints;
        /// <summary>
        /// 当存在为0的索引时将会重新计算索引
        /// </summary>
        protected IEnumerable<Endpoint> Endpoints
        {
            get => this.endpoints;
            set
            {
                if (value == null) return;

                var descriptors = value.ToArray();

                if (descriptors.All(s => s.Index == 0))
                {
                    var index = 0;
                    foreach (var item in descriptors.OrderBy(s => s.Path)
                                                    .ThenBy(s => s.Method)
                                                    .ThenBy(s => s.Index))
                    {
                        ++index;
                        item.Index = index;
                    }
                }

                this.endpoints = descriptors;
            }
        }
        public virtual Task<IEnumerable<Endpoint>> ListAsync()
        {
            return Task.FromResult(this.Endpoints);
        }
    }
}
