using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.MessageQueue;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.EventStore.Document
{
    /// <summary>
    /// 
    /// </summary>
    public static class DocumentExtension
    {
        /// <summary>
        /// 从文档中获得文档对象
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="documentStore"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static TDocument Find<TDocument>(this IDocumentStore documentStore, IIdentity identity) where TDocument : IDocument, new()
        {
            return documentStore.FindAsync<TDocument>(identity)
                                .GetAwaiter()
                                .GetResult();
        }

        /// <summary>
        /// 获取特点版本的文档对象
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="documentStore"></param>
        /// <param name="identity">标识</param>
        /// <param name="version">版本号</param>
        /// <returns></returns>
        public static TDocument Find<TDocument>(this IDocumentStore documentStore, IIdentity identity, int version) where TDocument : IDocument, new()
        {
            return documentStore.FindAsync<TDocument>(identity, version)
                                .GetAwaiter()
                                .GetResult();
        }

        /// <summary>
        /// 保存文档对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="document"></param>
        public static void Save<TDocument>(this IDocumentStore documentStore, TDocument document) where TDocument : IDocument
        {
            documentStore.SaveAsync(document)
                         .Wait();
        }


        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="identity"></param>
        public static void Remove<TDocument>(this IDocumentStore documentStore, IIdentity identity) where TDocument : IDocument
        {
            documentStore.RemoveAsync<TDocument>(identity)
                         .Wait();
        }

        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="document"></param>
        public static void Remove<TDocument>(this IDocumentStore documentStore, TDocument document) where TDocument : IDocument
        {
            documentStore.RemoveAsync(document)
                         .Wait();
        }

        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="document"></param>
        public static Task RemoveAsync<TDocument>(this IDocumentStore documentStore, TDocument document) where TDocument : IDocument
        {
            return documentStore.RemoveAsync<TDocument>(document.Identity);
        }


    }
}
