using System;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.EventStore.Document
{
    public class AbstractHandler<TDocument> where TDocument : IDocument, new()
    {
        protected readonly IDocumentStore _documentStore;

        public AbstractHandler(IDocumentStore documentStore)
        {
            this._documentStore = documentStore;
        }

        protected virtual async Task<TDocument> Add(TDocument document)
        {
            await this._documentStore.SaveAsync(document);
            return document;
        }

        protected virtual async Task Update(string identity, Action<TDocument> updateAction)
        {
            var document = await this._documentStore.FindAsync<TDocument>(identity);
            updateAction(document);
            await this._documentStore.SaveAsync(document);
        }

        protected virtual Task Remove(string identity)
        {
            return this._documentStore.DeleteAsync<TDocument>(identity);
        }

        protected virtual Task<TDocument> GetById(string identity)
        {
            return this._documentStore.FindAsync<TDocument>(identity);
        }
    }

    public class AbstractHandler
    {
        protected readonly IDocumentStore _documentStore;

        public AbstractHandler(IDocumentStore documentStore)
        {
            this._documentStore = documentStore;
        }

        protected virtual async Task<TDocument> Add<TDocument>(TDocument document) where TDocument : IDocument, new()
        {
            await this._documentStore.SaveAsync(document);
            return document;
        }

        protected virtual async Task Update<TDocument>(string identity, Action<TDocument> updateAction) where TDocument : IDocument, new()
        {
            var document = await this._documentStore.FindAsync<TDocument>(identity);
            updateAction(document);
            await this._documentStore.SaveAsync(document);
        }

        protected virtual Task Remove<TDocument>(string identity) where TDocument : IDocument, new()
        {
            return this._documentStore.DeleteAsync<TDocument>(identity);
        }

        protected virtual Task GetById<TDocument>(string identity) where TDocument : IDocument, new()
        {
            return this._documentStore.FindAsync<TDocument>(identity);
        }
    }
}
