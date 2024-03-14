using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAE.CommonLibrary.Data;

namespace SAE.CommonLibrary.EventStore.Document
{
    /// <summary>
    /// 数据持久化适配服务
    /// </summary>
    /// <typeparam name="TDocument">数据类型</typeparam>
    public class DefaultDataPersistenceServiceAdapter<TDocument> : IPersistenceService<TDocument> where TDocument : class, IDocument
    {
        private readonly IStorage _storage;
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="storage">存储接口</param>
        public DefaultDataPersistenceServiceAdapter(IStorage storage)
        {
            this._storage = storage;
        }
        /// <inheritdoc/>
        public async Task DeleteAsync(IIdentity identity)
        {
            await this._storage.DeleteAsync<TDocument>(identity.ToString());
        }
        /// <inheritdoc/>
        public async Task SaveAsync(TDocument document)
        {
            await this._storage.SaveAsync(document);
        }
    }
}
