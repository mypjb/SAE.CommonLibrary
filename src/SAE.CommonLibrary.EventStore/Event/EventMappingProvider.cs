using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore
{
    /// <summary>
    /// 事件映射提供器
    /// </summary>
    public class EventMappingProvider
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="types"></param>
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
