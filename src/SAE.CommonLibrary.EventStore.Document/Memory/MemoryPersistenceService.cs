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

        public Task AddAsync(TDocument document)
        {
            this._store.Add(document);
            return Task.FromResult(0);
        }

        public Task<TDocument> FindAsync(IIdentity identity)
        {
            return Task.FromResult(this._store.FirstOrDefault(s => s.Identity.Equals(identity)));
        }

        public Task RemoveAsync(TDocument docuemnt)
        {
            this._store.RemoveAll(s => s.Identity.Equals(docuemnt.Identity));
            return Task.FromResult(0);
        }

        public async Task SaveAsync(TDocument document)
        {
            var index = this._store.FindIndex(s => s.Identity.Equals(document.Identity));
            if (index == -1)
            {
                await this.AddAsync(document);
            }
            else
            {
                await this.UpdateAsync(document);
            }

        }

        public async Task UpdateAsync(TDocument document)
        {
            await this.RemoveAsync(document);
            await this.AddAsync(document);
        }
    }
}
