using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.CommonLibrary.EventStore.Document
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultDocumentEvent : IDocumentEvent
    {
        private readonly IServiceProvider _provider;
        private ConcurrentDictionary<Type, object> _dictionary;
        public DefaultDocumentEvent(IServiceProvider provider)
        {
            this._provider = provider;
            this._dictionary = new ConcurrentDictionary<Type, object>();
        }

        public async Task AppendAsync<TDocument>(TDocument document, IEnumerable<IEvent> events) where TDocument : IDocument
        {
            var persistenceService = this.GetService<TDocument>();

            await persistenceService.SaveAsync(document);
        }

        public async Task RemoveAsync<TDocument>(IIdentity identity) where TDocument : IDocument
        {
            var persistenceService = this.GetService<TDocument>();

            await persistenceService.RemoveAsync(identity);
        }

        private IPersistenceService<TDocument> GetService<TDocument>() where TDocument : IDocument
        {
            var key = typeof(TDocument);
            return this._dictionary.GetOrAdd(key, s =>
            {
                return this._provider.GetService<IPersistenceService<TDocument>>();
            }) as IPersistenceService<TDocument>;
        }
    }
}
