using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Data.MongoDB
{
    public interface IDescriptionProvider
    {
        void Add<T>(TableDescription<T> description) where T : class;
        TableDescription<T> Get<T>() where T : class;
    }

    internal class DescriptionProvider : IDescriptionProvider
    {
        private readonly ConcurrentDictionary<string, object> _pairs;
        public DescriptionProvider()
        {
            this._pairs = new ConcurrentDictionary<string, object>();
        }
        public void Add<T>(TableDescription<T> description) where T : class
        {
            this._pairs.AddOrUpdate(typeof(T).GUID.ToString(), s => description, (k, v) => description);
        }

        public TableDescription<T> Get<T>() where T : class
        {
            return (TableDescription<T>)this._pairs.GetOrAdd(typeof(T).GUID.ToString(), s => new TableDescription<T>());
        }
    }
}
