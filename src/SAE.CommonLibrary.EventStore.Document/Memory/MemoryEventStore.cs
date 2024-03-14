using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.CommonLibrary.EventStore;

namespace SAE.CommonLibrary.EventStore.Document.Memory
{
    /// <summary>
    /// 基于内存的事件存储
    /// </summary>
    public class MemoryEventStore : IEventStore
    {
        /// <summary>
        /// 事件流存储器
        /// </summary>
        private readonly ConcurrentDictionary<string, List<EventStream>> _store;
        /// <summary>
        /// ctor
        /// </summary>
        public MemoryEventStore()
        {
            _store = new ConcurrentDictionary<string, List<EventStream>>();
        }

        /// <inheritdoc/>
        public Task AppendAsync(EventStream eventStream)
        {
            var list = new List<EventStream>() { eventStream };
            this._store.AddOrUpdate(eventStream.Identity.ToString(), list, (k, v) =>
            {
                v.Add(eventStream);
                return v;
            });
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task<int> GetVersionAsync(IIdentity identity)
        {
            if (this._store.TryGetValue(identity.ToString(), out var list))
            {
                return list.ToArray().Where(s => s.Identity.Equals(identity))
                                     .OrderByDescending(s => s.Version)
                                     .FirstOrDefault()?.Version ?? 0;
            }
            return 0;
        }

        /// <inheritdoc/>
        public Task<EventStream> LoadEventStreamAsync(IIdentity identity, int skipEvents, int maxCount)
        {
            var eventStream = new EventStream(identity, 0, string.Empty);

            if (this._store.TryGetValue(identity.ToString(), out var list))
            {
                foreach (var @event in list.ToArray().Where(s => s.Identity.Equals(identity))
                                           .Skip((int)skipEvents)
                                           .Take(maxCount))
                {
                    eventStream.Append(@event);
                }
            }
            return Task.FromResult(eventStream);
        }

        /// <inheritdoc/>
        public Task DeleteAsync(IIdentity identity)
        {
            this._store.TryRemove(identity.ToString(), out _);
            return Task.CompletedTask;
        }
    }
}
