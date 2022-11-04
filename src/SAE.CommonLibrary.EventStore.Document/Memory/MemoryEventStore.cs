using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.CommonLibrary.EventStore;

namespace SAE.CommonLibrary.EventStore.Document.Memory
{
    /// <summary>
    /// 基于内存的事件存储
    /// </summary>
    /// <inheritdoc/>
    public class MemoryEventStore : IEventStore
    {
        /// <summary>
        /// 事件流存储器
        /// </summary>
        private readonly List<EventStream> _store;
        /// <summary>
        /// 
        /// </summary>
        public MemoryEventStore()
        {
            _store = new List<EventStream>();
        }
        public Task AppendAsync(EventStream eventStream)
        {
            _store.Add(eventStream);
            return Task.CompletedTask;
        }

        public Task<int> GetVersionAsync(IIdentity identity)
        {
            return Task.FromResult(_store.Where(s => s.Identity.Equals(identity))
                                   .OrderByDescending(s => s.Version)
                                   .FirstOrDefault()?.Version ?? 0);
        }

        public Task<EventStream> LoadEventStreamAsync(IIdentity identity, int skipEvents, int maxCount)
        {
            var eventStream = new EventStream(identity, 0, string.Empty);
            foreach (var @event in _store.Where(s => s.Identity.Equals(identity))
                                         .Skip((int)skipEvents)
                                         .Take(maxCount))
            {
                eventStream.Append(@event);
            }
            return Task.FromResult(eventStream);
        }

        public Task DeleteAsync(IIdentity identity)
        {
            this._store.RemoveAll(s => s.Identity.Equals(identity));
            return Task.CompletedTask;
        }
    }
}
