using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.MessageQueue;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.EventStore.Document
{
    /// <summary>
    /// <seealso cref="IDocumentStore"/>扩展
    /// </summary>
    public static class DocumentStoreExtension
    {
        /// <summary>
        /// 从文档中获得文档对象
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="documentStore"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static TDocument Find<TDocument>(this IDocumentStore documentStore, string identity) where TDocument : IDocument, new()
        {
            return documentStore.Find<TDocument>(identity.ToIdentity());
        }

        /// <summary>
        /// 从文档中获得文档对象
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="documentStore"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static TDocument Find<TDocument>(this IDocumentStore documentStore, string identity, int version) where TDocument : IDocument, new()
        {
            return documentStore.Find<TDocument>(identity.ToIdentity(), version);
        }
        /// <summary>
        /// 从文档中获得文档对象
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="documentStore"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static Task<TDocument> FindAsync<TDocument>(this IDocumentStore documentStore, string identity) where TDocument : IDocument, new()
        {
            return documentStore.FindAsync<TDocument>(identity.ToIdentity());
        }
        /// <summary>
        /// 从文档中获得文档对象
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="documentStore"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static Task<TDocument> FindAsync<TDocument>(this IDocumentStore documentStore, string identity,int version) where TDocument : IDocument, new()
        {
            return documentStore.FindAsync<TDocument>(identity.ToIdentity(), version);
        }
        /// <summary>
        /// 从文档中获得最新文档对象
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="documentStore"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static Task<TDocument> FindAsync<TDocument>(this IDocumentStore documentStore, IIdentity identity) where TDocument : IDocument, new()
        {
            return documentStore.FindAsync<TDocument>(identity, int.MaxValue);
        }

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
        public static void Save<TDocument>(this IDocumentStore documentStore, TDocument document) where TDocument : IDocument, new()
        {
            documentStore.SaveAsync(document)
                         .GetAwaiter()
                         .GetResult();
        }


        public static void Remove<TDocument>(this IDocumentStore documentStore, string identity) where TDocument : IDocument, new()
        {
            documentStore.Remove<TDocument>(identity.ToIdentity());
        }

        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="identity"></param>
        public static void Remove<TDocument>(this IDocumentStore documentStore, IIdentity identity) where TDocument : IDocument,new()
        {
            documentStore.RemoveAsync<TDocument>(identity)
                         .GetAwaiter()
                         .GetResult();
        }

        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="document"></param>
        public static void Remove<TDocument>(this IDocumentStore documentStore, TDocument document) where TDocument : IDocument, new()
        {
            documentStore.RemoveAsync(document)
                         .GetAwaiter()
                         .GetResult();
        }
        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="document"></param>
        public static Task RemoveAsync<TDocument>(this IDocumentStore documentStore, string identity) where TDocument : IDocument, new()
        {
            return documentStore.RemoveAsync<TDocument>(identity.ToIdentity());
        }

        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="document"></param>
        public static Task RemoveAsync<TDocument>(this IDocumentStore documentStore, TDocument document) where TDocument : IDocument, new()
        {
            return documentStore.RemoveAsync<TDocument>(document.Identity);
        }

    }
}
