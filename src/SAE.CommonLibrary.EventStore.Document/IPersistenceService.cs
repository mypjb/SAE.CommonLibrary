using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.EventStore.Document
{
    /// <summary>
    /// 持久化服务
    /// </summary>
    public interface IPersistenceService<TDocument> where TDocument : IDocument
    {
        /// <summary>
        /// 保存<typeparamref name="TDocument"/>
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        Task SaveAsync(TDocument document);
        /// <summary>
        /// 移除<typeparamref name="TDocument"/>
        /// </summary>
        /// <param name="docuemnt"></param>
        /// <returns></returns>
        Task RemoveAsync(IIdentity identity);
    }
}
