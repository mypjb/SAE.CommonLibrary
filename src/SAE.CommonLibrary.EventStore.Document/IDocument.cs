using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore.Document
{
    /// <summary>
    /// 文档对象
    /// </summary>
    public interface IDocument
    {
        /// <summary>
        /// 文档标识
        /// </summary>
        IIdentity Identity { get; }
        /// <summary>
        /// 通过给定的事件变更文档
        /// </summary>
        /// <param name="event">事件</param>
        void Mutate(IEvent @event);
        /// <summary>
        /// 更改的事件
        /// </summary>
        IEnumerable<IEvent> ChangeEvents { get; }
        /// <summary>
        /// 版本
        /// </summary>
        int Version { get; set; }
    }
}
