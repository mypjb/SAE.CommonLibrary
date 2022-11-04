using System;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.EventStore.Document
{
    /// <summary>
    /// <see cref="IDocument"/>基于范型的抽象处理基类
    /// </summary>
    /// <typeparam name="TDocument">文档类型</typeparam>
    public class AbstractHandler<TDocument> where TDocument : IDocument, new()
    {
        protected readonly IDocumentStore _documentStore;
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="documentStore"></param>
        public AbstractHandler(IDocumentStore documentStore)
        {
            this._documentStore = documentStore;
        }
        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="document">文档对象</param>
        protected virtual async Task<TDocument> AddAsync(TDocument document)
        {
            await this._documentStore.SaveAsync(document);
            return document;
        }
        /// <summary>
        /// 更新操作
        /// </summary>
        /// <param name="identity">文档标识</param>
        /// <param name="updateAction">更新前的操作</param>
        protected virtual async Task UpdateAsync(string identity, Action<TDocument> updateAction)
        {
            var document = await this._documentStore.FindAsync<TDocument>(identity);
            updateAction(document);
            await this._documentStore.SaveAsync(document);
        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="identity">文档标识</param>
        protected virtual Task DeleteAsync(string identity)
        {
            return this._documentStore.DeleteAsync<TDocument>(identity);
        }
        /// <summary>
        /// 查找文档对象
        /// </summary>
        /// <param name="identity">文档标识</param>
        protected virtual Task<TDocument> FindAsync(string identity)
        {
            return this._documentStore.FindAsync<TDocument>(identity);
        }
    }
    /// <summary>
    /// <see cref="IDocument"/>抽象处理基类
    /// </summary>
    public class AbstractHandler
    {
        protected readonly IDocumentStore _documentStore;
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="documentStore"></param>
        public AbstractHandler(IDocumentStore documentStore)
        {
            this._documentStore = documentStore;
        }
        /// <summary>
        /// 添加文档到持久存储
        /// </summary>
        /// <param name="document">要新增的文档</param>
        /// <typeparam name="TDocument">文档类型</typeparam>
        protected virtual async Task<TDocument> AddAsync<TDocument>(TDocument document) where TDocument : IDocument, new()
        {
            await this._documentStore.SaveAsync(document);
            return document;
        }
        /// <summary>
        /// 更新操作
        /// </summary>
        /// <param name="identity">文档标识</param>
        /// <param name="updateAction">更新前的操作</param>
        /// <typeparam name="TDocument">文档类型</typeparam>
        protected virtual async Task UpdateAsync<TDocument>(string identity, Action<TDocument> updateAction) where TDocument : IDocument, new()
        {
            var document = await this._documentStore.FindAsync<TDocument>(identity);
            updateAction(document);
            await this._documentStore.SaveAsync(document);
        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="identity">文档标识</param>
        /// <typeparam name="TDocument">文档类型</typeparam>
        protected virtual Task DeleteAsync<TDocument>(string identity) where TDocument : IDocument, new()
        {
            return this._documentStore.DeleteAsync<TDocument>(identity);
        }
        /// <summary>
        /// 查找文档
        /// </summary>
        /// <param name="identity">文档标识</param>
        /// <typeparam name="TDocument">文档类型</typeparam>
        protected virtual Task FindAsync<TDocument>(string identity) where TDocument : IDocument, new()
        {
            return this._documentStore.FindAsync<TDocument>(identity);
        }
    }
}
