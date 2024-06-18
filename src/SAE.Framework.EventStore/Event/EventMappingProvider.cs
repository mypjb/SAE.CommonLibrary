using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.EventStore
{
    /// <summary>
    /// 事件映射提供器
    /// </summary>
    public class EventMappingProvider
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="types">事件类型集合</param>
        public EventMappingProvider(IEnumerable<Type> types)
        {
            this.Types = types;
        }
        /// <summary>
        /// 事件类型列表
        /// </summary>
        /// <value></value>
        public IEnumerable<Type> Types { get; }
    }
}
