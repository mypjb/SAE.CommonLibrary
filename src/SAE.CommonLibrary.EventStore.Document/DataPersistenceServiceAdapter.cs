using SAE.CommonLibrary.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace SAE.CommonLibrary.EventStore.Document
{
    /// <summary>
    /// 数据持久化适配服务
    /// </summary>
    /// <typeparam name="TDocument">数据类型</typeparam>
    /// <inheritdoc/>
    public class DataPersistenceServiceAdapter<TDocument> : IPersistenceService<TDocument> where TDocument : class, IDocument
    {
        private readonly IStorage _storage;
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="storage"></param>
        public DataPersistenceServiceAdapter(IStorage storage)
        {
            this._storage = storage;
        }
        public Task DeleteAsync(IIdentity identity)
        {
            return this._storage.DeleteAsync<TDocument>(identity.ToString());
        }

        public Task SaveAsync(TDocument document)
        {
            return this._storage.SaveAsync(document);
        }
    }
}
