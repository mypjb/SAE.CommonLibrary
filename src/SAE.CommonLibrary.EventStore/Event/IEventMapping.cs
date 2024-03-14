using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore
{
    /// <summary>
    /// 事件映射接口
    /// </summary>
    public interface IEventMapping
    {
        /// <summary>
        /// 添加映射
        /// </summary>
        /// <param name="key">映射的key</param>
        /// <param name="type">映射的类型</param>
        void Add(string key, Type type);
        /// <summary>
        /// 通过key获得映射
        /// </summary>
        /// <param name="key">映射的key</param>
        /// <returns>返回key对应的类型</returns>
        Type Get(string key);
    }

    /// <summary>
    /// <see cref="IEventMapping"/> 默认实现
    /// </summary>
    public class DefaultEventMapping : IEventMapping
    {
        private readonly ILogging<DefaultEventMapping> _logging;
        private readonly Dictionary<string, Type> _mapping;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logging">日志记录器</param>
        /// <param name="providers">事件映射提供程序</param>
        public DefaultEventMapping(ILogging<DefaultEventMapping> logging,IEnumerable<EventMappingProvider> providers)
        {
            this._logging = logging;
            this._mapping = new Dictionary<string, Type>();
            foreach (var provider in providers)
            {
                foreach (var type in provider.Types)
                {
                    this.Add(type.GetIdentity(), type);
                }
            }
        }
        /// <inheritdoc/>
        public void Add(string key, Type type)
        {
            this._mapping[key] = type;
            this._logging.Info($"add mapping: key:'{key}',type:{type}");
        }
        /// <inheritdoc/>
        public Type Get(string key)
        {
            Type type;

            if (!this._mapping.TryGetValue(key, out type))
            {
                var message = $"not exist '{key}' the mapping";
                this._logging.Error(message);
                throw new SAEException(StatusCodes.ResourcesExist, message);
            }

            return type;
        }
    }
}
