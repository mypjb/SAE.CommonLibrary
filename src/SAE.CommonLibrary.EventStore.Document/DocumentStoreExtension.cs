using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Extension;
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
        /// <param name="id"></param>
        /// <returns></returns>
        public static TDocument Find<TDocument>(this IDocumentStore documentStore, string id) where TDocument : IDocument, new()
        {
            return documentStore.Find<TDocument>(id.ToIdentity());
        }

        /// <summary>
        /// 从文档中获得文档对象
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="documentStore"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TDocument Find<TDocument>(this IDocumentStore documentStore, string id, int version) where TDocument : IDocument, new()
        {
            return documentStore.Find<TDocument>(id.ToIdentity(), version);
        }
        /// <summary>
        /// 从文档中获得文档对象
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="documentStore"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Task<TDocument> FindAsync<TDocument>(this IDocumentStore documentStore, string id) where TDocument : IDocument, new()
        {
            return documentStore.FindAsync<TDocument>(id.ToIdentity());
        }
        /// <summary>
        /// 从文档中获得文档对象
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="documentStore"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Task<TDocument> FindAsync<TDocument>(this IDocumentStore documentStore, string id, int version) where TDocument : IDocument, new()
        {
            return documentStore.FindAsync<TDocument>(id.ToIdentity(), version);
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

        /// <summary>
        /// 保存文档对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="documents"></param>
        public static void Save<TDocument>(this IDocumentStore documentStore,IEnumerable<TDocument> documents) where TDocument : IDocument, new()
        {
            documentStore.SaveAsync(documents)
                         .GetAwaiter()
                         .GetResult();
        }


        /// <summary>
        /// 保存文档对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="documents"></param>
        public static Task SaveAsync<TDocument>(this IDocumentStore documentStore, IEnumerable<TDocument> documents) where TDocument : IDocument, new()
        {
            return documents.ForEachAsync(documentStore.SaveAsync);
        }

        /// <summary>
        /// delete document
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="documentStore"></param>
        /// <param name="id"></param>
        public static void Delete<TDocument>(this IDocumentStore documentStore, string id) where TDocument : IDocument, new()
        {
            documentStore.Delete<TDocument>(id.ToIdentity());
        }

        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="identity"></param>
        public static void Delete<TDocument>(this IDocumentStore documentStore, IIdentity identity) where TDocument : IDocument,new()
        {
            documentStore.DeleteAsync<TDocument>(identity)
                         .GetAwaiter()
                         .GetResult();
        }

        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="document"></param>
        public static void Delete<TDocument>(this IDocumentStore documentStore, TDocument document) where TDocument : IDocument, new()
        {
            documentStore.DeleteAsync(document)
                         .GetAwaiter()
                         .GetResult();
        }

        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="documents"></param>
        public static void Delete<TDocument>(this IDocumentStore documentStore, IEnumerable<TDocument> documents) where TDocument : IDocument, new()
        {
            documentStore.DeleteAsync(documents)
                         .GetAwaiter()
                         .GetResult();
        }

        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="documents"></param>
        public static async Task DeleteAsync<TDocument>(this IDocumentStore documentStore, IEnumerable<TDocument> documents) where TDocument : IDocument, new()
        {
            await documents.ForEachAsync(documentStore.DeleteAsync);
        }


        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="id"></param>
        public static Task DeleteAsync<TDocument>(this IDocumentStore documentStore, string id) where TDocument : IDocument, new()
        {
            return documentStore.DeleteAsync<TDocument>(id.ToIdentity());
        }

        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="document"></param>
        public static Task DeleteAsync<TDocument>(this IDocumentStore documentStore, TDocument document) where TDocument : IDocument, new()
        {
            return documentStore.DeleteAsync<TDocument>(document.Identity);
        }

        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="ids"></param>
        public static void Delete<TDocument>(this IDocumentStore documentStore, IEnumerable<string> ids) where TDocument : IDocument, new()
        {
            documentStore.DeleteAsync<TDocument>(ids)
                         .GetAwaiter()
                         .GetResult();
        }

        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="ids"></param>
        public static Task DeleteAsync<TDocument>(this IDocumentStore documentStore, IEnumerable<string> ids) where TDocument : IDocument, new()
        {
            return ids.ForEachAsync(documentStore.DeleteAsync<TDocument>);
        }

        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="identitys"></param>
        public static void Delete<TDocument>(this IDocumentStore documentStore, IEnumerable<Identity> identitys) where TDocument : IDocument, new()
        {
            documentStore.DeleteAsync<TDocument>(identitys)
                         .GetAwaiter()
                         .GetResult();
        }

        /// <summary>
        /// 从<seealso cref="IDocumentStore"/>中移除该对象
        /// </summary>
        /// <param name="documentStore"></param>
        /// <param name="identitys"></param>
        public static Task DeleteAsync<TDocument>(this IDocumentStore documentStore, IEnumerable<Identity> identitys) where TDocument : IDocument, new()
        {
            return identitys.ForEachAsync(documentStore.DeleteAsync<TDocument>);
        }

    }
}
