using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.EventStore.Document
{
    /// <summary>
    /// 文档存储
    /// </summary>
    public interface IDocumentStore
    {

        /// <summary>
        /// 获取特定版本的文档对象
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="identity">标识</param>
        /// <param name="version">版本号</param>
        /// <returns>文档</returns>
        Task<TDocument> FindAsync<TDocument>(IIdentity identity, int version) where TDocument : IDocument, new();

        /// <summary>
        /// 保存文件操作事件
        /// </summary>
        /// <param name="document">文档</param>
        Task SaveAsync<TDocument>(TDocument document) where TDocument : IDocument, new();

        /// <summary>
        /// 使用<seealso cref="IIdentity"/>移除文档对象
        /// </summary>
        /// <param name="identity">标识</param>
        Task DeleteAsync<TDocument>(IIdentity identity) where TDocument : IDocument, new();
    }
}
