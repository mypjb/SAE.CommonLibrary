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
    /// <see cref="IDocumentStore"/>扩展
    /// </summary>
    public static class DocumentStoreExtension
    {
        /// <summary>
        /// 获得文档对象
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="id">文档标识</param>
        /// <returns>文档对象</returns>
        public static TDocument Find<TDocument>(this IDocumentStore documentStore, string id) where TDocument : IDocument, new()
        {
            return documentStore.Find<TDocument>(id.ToIdentity());
        }

        /// <summary>
        /// 获得指定版本的文档对象
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="id">文档标识</param>
        /// <param name="version">版本号</param>
        /// <returns>文档对象</returns>
        public static TDocument Find<TDocument>(this IDocumentStore documentStore, string id, int version) where TDocument : IDocument, new()
        {
            return documentStore.Find<TDocument>(id.ToIdentity(), version);
        }
        /// <summary>
        /// 文档对象
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="id">文档标识</param>
        /// <returns>文档对象</returns>
        public static async Task<TDocument> FindAsync<TDocument>(this IDocumentStore documentStore, string id) where TDocument : IDocument, new()
        {
            return await documentStore.FindAsync<TDocument>(id.ToIdentity());
        }
        /// <summary>
        /// 获得指定版本的文档对象
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="id">文档标识</param>
        /// <param name="version">版本号</param>
        /// <returns>文档对象</returns>
        public static async Task<TDocument> FindAsync<TDocument>(this IDocumentStore documentStore, string id, int version) where TDocument : IDocument, new()
        {
            return await documentStore.FindAsync<TDocument>(id.ToIdentity(), version);
        }
        /// <summary>
        /// 获得最新文档对象
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="identity">文档标识</param>
        /// <returns>文档对象</returns>
        public static async Task<TDocument> FindAsync<TDocument>(this IDocumentStore documentStore, IIdentity identity) where TDocument : IDocument, new()
        {
            return await documentStore.FindAsync<TDocument>(identity, int.MaxValue);
        }

        /// <summary>
        /// 获得文档对象
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="identity">文档标识</param>
        /// <returns>文档对象</returns>
        public static TDocument Find<TDocument>(this IDocumentStore documentStore, IIdentity identity) where TDocument : IDocument, new()
        {
            return documentStore.FindAsync<TDocument>(identity)
                                .GetAwaiter()
                                .GetResult();
        }

        /// <summary>
        /// 获得指定版本的文档对象
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="identity">标识</param>
        /// <param name="version">版本号</param>
        /// <returns>文档对象</returns>
        public static TDocument Find<TDocument>(this IDocumentStore documentStore, IIdentity identity, int version) where TDocument : IDocument, new()
        {
            return documentStore.FindAsync<TDocument>(identity, version)
                                .GetAwaiter()
                                .GetResult();
        }

        /// <summary>
        /// 保存文档对象
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="document">文档对象</param>
        public static void Save<TDocument>(this IDocumentStore documentStore, TDocument document) where TDocument : IDocument, new()
        {
            documentStore.SaveAsync(document)
                         .GetAwaiter()
                         .GetResult();
        }

        /// <summary>
        /// 保存文档对象
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="documents">文档对象集合</param>
        public static void Save<TDocument>(this IDocumentStore documentStore,IEnumerable<TDocument> documents) where TDocument : IDocument, new()
        {
            documentStore.SaveAsync(documents)
                         .GetAwaiter()
                         .GetResult();
        }


        /// <summary>
        /// 保存文档对象
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="documents">文档对象集合</param>
        public static Task SaveAsync<TDocument>(this IDocumentStore documentStore, IEnumerable<TDocument> documents) where TDocument : IDocument, new()
        {
            return documents.ForEachAsync(documentStore.SaveAsync);
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="id">文档标识</param>
        public static void Delete<TDocument>(this IDocumentStore documentStore, string id) where TDocument : IDocument, new()
        {
            documentStore.Delete<TDocument>(id.ToIdentity());
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="identity">文档标识</param>
        public static void Delete<TDocument>(this IDocumentStore documentStore, IIdentity identity) where TDocument : IDocument,new()
        {
            documentStore.DeleteAsync<TDocument>(identity)
                         .GetAwaiter()
                         .GetResult();
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="document">文档对象</param>
        public static void Delete<TDocument>(this IDocumentStore documentStore, TDocument document) where TDocument : IDocument, new()
        {
            documentStore.DeleteAsync(document)
                         .GetAwaiter()
                         .GetResult();
        }

        /// <summary>
        /// 批量删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="documents">文档对象集合</param>
        public static void Delete<TDocument>(this IDocumentStore documentStore, IEnumerable<TDocument> documents) where TDocument : IDocument, new()
        {
            documentStore.DeleteAsync(documents)
                         .GetAwaiter()
                         .GetResult();
        }

        /// <summary>
        /// 批量删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="documents">文档对象集合</param>
        public static async Task DeleteAsync<TDocument>(this IDocumentStore documentStore, IEnumerable<TDocument> documents) where TDocument : IDocument, new()
        {
            await documents.ForEachAsync(documentStore.DeleteAsync);
        }


        /// <summary>
        /// 删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="id">标识</param>
        public static Task DeleteAsync<TDocument>(this IDocumentStore documentStore, string id) where TDocument : IDocument, new()
        {
            return documentStore.DeleteAsync<TDocument>(id.ToIdentity());
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="document">文档对象</param>
        public static Task DeleteAsync<TDocument>(this IDocumentStore documentStore, TDocument document) where TDocument : IDocument, new()
        {
            return documentStore.DeleteAsync<TDocument>(document.Identity);
        }

        /// <summary>
        /// 批量删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="ids">标识集合</param>
        public static void Delete<TDocument>(this IDocumentStore documentStore, IEnumerable<string> ids) where TDocument : IDocument, new()
        {
            documentStore.DeleteAsync<TDocument>(ids)
                         .GetAwaiter()
                         .GetResult();
        }

        /// <summary>
        /// 批量删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="ids">标识集合</param>
        public static Task DeleteAsync<TDocument>(this IDocumentStore documentStore, IEnumerable<string> ids) where TDocument : IDocument, new()
        {
            return ids.ForEachAsync(documentStore.DeleteAsync<TDocument>);
        }

        /// <summary>
        /// 批量删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="identitys">标识集合</param>
        public static void Delete<TDocument>(this IDocumentStore documentStore, IEnumerable<Identity> identitys) where TDocument : IDocument, new()
        {
            documentStore.DeleteAsync<TDocument>(identitys)
                         .GetAwaiter()
                         .GetResult();
        }

        /// <summary>
        /// 批量删除文档
        /// </summary>
        /// <typeparam name="TDocument">文档类型</typeparam>
        /// <param name="documentStore">文档存储接口</param>
        /// <param name="identitys">标识集合</param>
        public static Task DeleteAsync<TDocument>(this IDocumentStore documentStore, IEnumerable<Identity> identitys) where TDocument : IDocument, new()
        {
            return identitys.ForEachAsync(documentStore.DeleteAsync<TDocument>);
        }

    }
}
