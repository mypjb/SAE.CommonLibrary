﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.EventStore.Document
{
    /// <summary>
    /// 文档事件
    /// </summary>
    public interface IDocumentEvent
    {
        /// <summary>
        /// 附加事件
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="events">附加事件</param>
        Task AppendAsync<TDocument>(TDocument document, IEnumerable<IEvent> events) where TDocument:IDocument;
        /// <summary>
        /// 移除文档对象
        /// </summary>
        /// <param name="identity">对象标识</param>
        Task DeleteAsync<TDocument>(IIdentity identity) where TDocument : IDocument;
    }
}
