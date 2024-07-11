using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.EventStore.Document
{
    /// <summary>
    /// 持久化扩展接口
    /// </summary>
    public static class PersistenceServiceExtension
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="TDocument"></typeparam>
        ///// <param name="persistenceService"></param>
        ///// <param name="identity"></param>
        ///// <returns></returns>
        //public static TDocument Find<TDocument>(this IPersistenceService<TDocument> persistenceService, IIdentity identity) where TDocument : IDocument
        //{
        //    return persistenceService.FindAsync(identity).GetAwaiter().GetResult();
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="TDocument"></typeparam>
        ///// <param name="persistenceService"></param>
        ///// <param name="identity"></param>
        ///// <returns></returns>
        //public static TDocument Find<TDocument>(this IPersistenceService<TDocument> persistenceService, string identity) where TDocument : IDocument
        //{
        //    return persistenceService.FindAsync<TDocument>(identity).GetAwaiter().GetResult();
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="TDocument"></typeparam>
        ///// <param name="persistenceService"></param>
        ///// <param name="identity"></param>
        ///// <returns></returns>
        //public static Task<TDocument> FindAsync<TDocument>(this IPersistenceService<TDocument> persistenceService, string identity) where TDocument : IDocument
        //{
        //    return persistenceService.FindAsync(identity.ToIdentity());
        //}
    }
}
