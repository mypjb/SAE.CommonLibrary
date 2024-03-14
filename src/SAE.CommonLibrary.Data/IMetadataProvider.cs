using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using SAE.CommonLibrary.DependencyInjection;

namespace SAE.CommonLibrary.Data
{
    /// <summary>
    /// 元数据提供程序
    /// </summary>
    public interface IMetadataProvider
    {
        /// <summary>
        /// 获得元数据
        /// </summary>
        /// <typeparam name="T">元数据对应的类型</typeparam>
        /// <returns>元数据对象</returns>
        Metadata<T> Get<T>() where T : class;
    }
    /// <summary>
    /// 默认的<see cref="IMetadataProvider"/>实现
    /// </summary>
    public class DefaultMetadataProvider : IMetadataProvider
    {
        private readonly ConcurrentDictionary<string, object> _pairs;
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        public DefaultMetadataProvider(IServiceProvider serviceProvider)
        {
            this._pairs = new ConcurrentDictionary<string, object>();
            this._serviceProvider = serviceProvider;
        }
        ///<inheritdoc/>
        public Metadata<T> Get<T>() where T : class
        {
            return (Metadata<T>)this._pairs.GetOrAdd(typeof(T).GUID.ToString(), s =>
            {
                Metadata<T> metadata;
                if (!_serviceProvider.TryGetService(out metadata))
                {
                    metadata = new Metadata<T>();
                }
                return metadata;
            });
        }
    }
}
