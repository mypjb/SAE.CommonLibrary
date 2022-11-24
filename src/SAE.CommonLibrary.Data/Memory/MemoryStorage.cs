using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.Data.Memory
{
    /// <summary>
    /// 内存存储
    /// </summary>
    public class MemoryStorage : IStorage
    {
        private readonly static object _lock = new object();
        private readonly ConcurrentDictionary<string, object> _storage;
        private readonly ILogging _logging;
        private readonly IMetadataProvider _metadataProvider;

        public MemoryStorage(ILogging<MemoryStorage> logging, IMetadataProvider metadataProvider)
        {
            this._logging = logging;
            this._metadataProvider = metadataProvider;
            this._logging.Warn("您正在使用基于内存的持久化实现，请勿在非开发环境下使用它");
            this._storage = new ConcurrentDictionary<string, object>();
        }


        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return this.GetStoreage<T>()
                       .Values
                       .AsQueryable();
        }


        public Task DeleteAsync<T>(T model) where T : class
        {
            var metadata = this._metadataProvider.Get<T>();
            return this.DeleteAsync<T>(metadata.IdentiyFactory(model));
        }

        public Task DeleteAsync<T>(object id) where T : class
        {
            this.GetSource<T>().Remove(id, out object _);

            return Task.CompletedTask;
        }



        private Dictionary<object, T> GetStoreage<T>() where T : class
        {
            var dictionary = this.GetSource<T>() as IDictionary;

            Dictionary<object, T> pairs = new Dictionary<object, T>();

            foreach (object key in dictionary.Keys)
            {
                pairs[key] = dictionary[key].To<T>();
            }

            return pairs;
        }

        private ConcurrentDictionary<object, object> GetSource<T>() where T : class
        {
            var metadata = this._metadataProvider.Get<T>();
            var dictionary = this._storage.GetOrAdd(metadata.Name, key =>
            {
                return new ConcurrentDictionary<object, object>();
            }) as ConcurrentDictionary<object, object>;
            return dictionary;
        }

        public Task SaveAsync<T>(T model) where T : class
        {
            var id = this._metadataProvider.Get<T>().IdentiyFactory(model);
            this.GetSource<T>().AddOrUpdate(id, model, (k, v) => model);
            return Task.CompletedTask;
        }



        #region Add And Update
        //public Task AddAsync<T>(T model) where T : class
        //{
        //    var id = this._metadataProvider.Get<T>().IdentiyFactory(model);

        //    this.GetSource<T>()
        //        .Add(id, model);

        //    this._logging.Info("Add - {0}: {1}", model.GetType().Name, model);

        //    return Task.CompletedTask;
        //}

        //public async Task UpdateAsync<T>(T model) where T : class
        //{
        //    await this.RemoveAsync(model);
        //    await this.AddAsync(model);
        //} 
        #endregion
    }
}
