using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.CommonLibrary.EventStore.Snapshot;

namespace SAE.CommonLibrary.EventStore.Document.Memory
{
    /// <summary>
    /// 基于内存的快照存储
    /// </summary>
    public class MemorySnapshotStore : ISnapshotStore
    {
        /// <summary>
        /// 快照列表
        /// </summary>
        private readonly ConcurrentDictionary<string, List<Snapshot.Snapshot>> _store;
        /// <summary>
        /// 
        /// </summary>
        public MemorySnapshotStore()
        {
            _store = new ConcurrentDictionary<string, List<Snapshot.Snapshot>>();
        }
        public Task<Snapshot.Snapshot> FindAsync(IIdentity identity, int version)
        {
            return Task.FromResult(this.GetStore(identity.ToString()).FirstOrDefault(s => s.Id == identity.ToString() && s.Version == version));
        }

        public Task<Snapshot.Snapshot> FindAsync(IIdentity identity)
        {
            return Task.FromResult(this.GetStore(identity.ToString()).Where(s => s.Id == identity.ToString())
                                         .OrderByDescending(s => s.Version)
                                         .FirstOrDefault());
        }

        public Task DeleteAsync(IIdentity identity)
        {
            this.GetStore(identity.ToString()).RemoveAll(s => s.Id == identity.ToString());
            return Task.CompletedTask;
        }

        public Task SaveAsync(Snapshot.Snapshot snapshot)
        {
            this.GetStore(snapshot.Id).Add(snapshot);
            return Task.CompletedTask;
        }

        private List<Snapshot.Snapshot> GetStore(string identity)
        {
            return this._store.GetOrAdd(identity, s => new List<Snapshot.Snapshot>());
        }
    }
}
