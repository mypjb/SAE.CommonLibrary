using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using SAE.CommonLibrary.DependencyInjection;

namespace SAE.CommonLibrary.Data
{
    public interface IMetadataProvider
    {
        Metadata<T> Get<T>() where T : class;
    }

    public class MetadataProvider : IMetadataProvider
    {
        private readonly ConcurrentDictionary<string, object> _pairs;
        private readonly IServiceProvider _serviceProvider;

        public MetadataProvider(IServiceProvider serviceProvider)
        {
            this._pairs = new ConcurrentDictionary<string, object>();
            this._serviceProvider = serviceProvider;
        }

        public Metadata<T> Get<T>() where T : class
        {
            return (Metadata<T>)this._pairs.GetOrAdd(typeof(T).GUID.ToString(), s=>
            {
                Metadata<T> metadata;
                if(!_serviceProvider.TryGetService(out metadata))
                {
                    metadata = new Metadata<T>();
                }
                return metadata;
            });
        }
    }
}
