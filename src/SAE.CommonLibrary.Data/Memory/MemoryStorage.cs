using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Data.Memory
{
    /// <summary>
    /// 内存存储
    /// </summary>
    public class MemoryStorage : IStorage
    {
        private readonly static object _lock = new object();
        private static readonly Dictionary<Type, object> _storage = new Dictionary<Type, object>();
        private readonly ILogging _logging;
        public MemoryStorage(ILogging<MemoryStorage> logging)
        {
            this._logging = logging;
        }
        public Task AddAsync<T>(T model)
        {
            var id = ((dynamic)model).Id;

            this.GetStoreage<T>()
                .Add(id, model);

            this._logging.Info("Add - {0}: {1}", model.GetType().Name, model);

            return Task.CompletedTask;
        }

        public IQueryable<T> AsQueryable<T>()
        {
            return this.GetStoreage<T>()
                       .Values
                       .AsQueryable();
        }

        
        public Task RemoveAsync<T>(T model)
        {
            var storage = this.GetStoreage<T>();

            if (storage.ContainsValue(model))
            {
                var kv = storage.First(s => s.Value.Equals(model));
                this._logging.Info("Remove Key “{0}” Model:{1}", kv.Key, kv.Value);
                storage.Remove(kv.Key);
            }
            return Task.CompletedTask;

        }

        public Task UpdateAsync<T>(T model)
        {
            return Task.CompletedTask;
        }

        private Dictionary<object, T> GetStoreage<T>()
        {
            var type = typeof(T);

            object o;

            if (!_storage.TryGetValue(type, out o))
            {
                lock (_lock)
                {
                    if (!_storage.TryGetValue(type, out o))
                    {
                        o = new Dictionary<object, T>();
                        _storage.Add(type, o);
                    }
                }
            }

            return o as Dictionary<object, T>;
        }
    }
}
