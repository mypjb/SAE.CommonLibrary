using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.CommonLibrary.EventStore.Document
{
    /// <summary>
    /// <see cref="IDocumentEvent"/>默认实现
    /// </summary>
    public class DefaultDocumentEvent : IDocumentEvent
    {
        private readonly IServiceProvider _provider;
        private readonly ConcurrentDictionary<Type, object> _dictionary;
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public DefaultDocumentEvent(IServiceProvider provider)
        {
            this._provider = provider;
            this._dictionary = new ConcurrentDictionary<Type, object>();
        }

        /// <inheritdoc/>
        public async Task AppendAsync<TDocument>(TDocument document, IEnumerable<IEvent> events) where TDocument : IDocument
        {
            var persistenceService = this.GetService<TDocument>();

            await persistenceService.SaveAsync(document);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync<TDocument>(IIdentity identity) where TDocument : IDocument
        {
            var persistenceService = this.GetService<TDocument>();

            await persistenceService.DeleteAsync(identity);
        }
        /// <summary>
        /// 获得<see cref="IPersistenceService{TDocument}"/>持久化服务
        /// </summary>
        /// <typeparam name="TDocument">文档对象</typeparam>
        /// <returns>持久化服务接口</returns>
        private IPersistenceService<TDocument> GetService<TDocument>() where TDocument : IDocument
        {
            var key = typeof(TDocument);
            return (IPersistenceService<TDocument>)this._dictionary.GetOrAdd(key, s =>
            {
                return this._provider.GetService<IPersistenceService<TDocument>>();
            });
        }
    }
}
