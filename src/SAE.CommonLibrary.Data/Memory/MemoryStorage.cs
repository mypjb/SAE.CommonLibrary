using SAE.CommonLibrary.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAE.CommonLibrary.Data.Memory
{
    /// <summary>
    /// 内存存储
    /// </summary>
    public class MemoryStorage : IStorage
    {
        private readonly static object _lock = new object();
        private static readonly Dictionary<Type, object> _storage = new Dictionary<Type, object>();
        private readonly ILog _log;
        public MemoryStorage(ILog<MemoryStorage> log)
        {
            this._log = log;
        }
        public void Add<T>(T model)
        {
            var id = ((dynamic)model).Id;

            this.GetStoreage<T>()
                .Add(id, model);

            this._log.Info("Add - {0}: {1}", model.GetType().Name, model);
        }

        public IQueryable<T> AsQueryable<T>()
        {
            return this.GetStoreage<T>()
                       .Values
                       .AsQueryable();
        }

        public T Find<T>(object id)
        {
            T value;
            if (!this.GetStoreage<T>()
                    .TryGetValue(id, out value))
            {
                this._log.Info("Find Id “{0}” Not Exist", id);
            }

            return value;
        }

        public void Remove<T>(T model)
        {
            var storage = this.GetStoreage<T>();

            if (storage.ContainsValue(model))
            {
                var kv = storage.First(s => s.Value.Equals(model));
                this._log.Info("Remove Key “{0}” Model:{1}", kv.Key, kv.Value);
                storage.Remove(kv.Key);
            }

        }

        public void Update<T>(T model)
        {

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
