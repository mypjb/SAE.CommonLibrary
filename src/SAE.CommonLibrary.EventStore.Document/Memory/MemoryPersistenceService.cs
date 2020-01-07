using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.EventStore.Document.Default
{
    public class MemoryPersistenceService<TDocument> : IPersistenceService<TDocument> where TDocument : IDocument
    {
        private readonly List<TDocument> _store;
        public MemoryPersistenceService()
        {
            this._store = new List<TDocument>();
        }


        public Task RemoveAsync(IIdentity identity)
        {
            this._store.RemoveAll(s => s.Identity.Equals(identity));
            return Task.FromResult(0);
        }

        public async Task SaveAsync(TDocument document)
        {
            var index = this._store.FindIndex(s => s.Identity.Equals(document.Identity));
            if (index != -1)
            {
                await this.RemoveAsync(document.Identity);
            }
            this._store.Add(document);
        }
    }
}
