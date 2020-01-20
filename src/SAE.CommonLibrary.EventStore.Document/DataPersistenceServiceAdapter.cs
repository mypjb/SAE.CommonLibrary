using SAE.CommonLibrary.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace SAE.CommonLibrary.EventStore.Document
{
    public class DataPersistenceServiceAdapter<TDocument> : IPersistenceService<TDocument> where TDocument : class, IDocument
    {
        private readonly IStorage _storage;

        public DataPersistenceServiceAdapter(IStorage storage)
        {
            this._storage = storage;
        }
        public Task RemoveAsync(IIdentity identity)
        {
            return this._storage.RemoveAsync<TDocument>(identity.ToString());
        }

        public Task SaveAsync(TDocument document)
        {
            return this._storage.SaveAsync(document);
        }
    }
}
