using System;

namespace SAE.CommonLibrary.EventStore
{
    /// <summary>
    /// 事件接口
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// 标识
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        long Version { get; set; }
    }
}