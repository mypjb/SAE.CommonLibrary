using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Data
{

    /// <summary>
    /// 存储构建对象
    /// </summary>
    public class StorageBuilder
    {
        /// <summary>
        /// 服务集合
        /// </summary>
        internal IServiceCollection ServiceCollection { get; }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceCollection">服务集合</param>
        public StorageBuilder(IServiceCollection serviceCollection)
        {
            this.ServiceCollection = serviceCollection;
        }
    }
}
