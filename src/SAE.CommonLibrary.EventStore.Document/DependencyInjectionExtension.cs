using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Data;
using SAE.CommonLibrary.EventStore;
using SAE.CommonLibrary.EventStore.Document;
using SAE.CommonLibrary.EventStore.Document.Memory;
using SAE.CommonLibrary.EventStore.Snapshot;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// 添加EventStore.Docment默认实现
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddDocument(this IServiceCollection serviceCollection)
        {
            if (!serviceCollection.IsRegister<IDocumentStore>())
            {
                serviceCollection.AddSaeOptions<DocumentConfig>();
                serviceCollection.TryAddSingleton<IDocumentStore, DefaultDocumentStore>();
                serviceCollection.TryAddSingleton<IDocumentEvent, DefaultDocumentEvent>();
                serviceCollection.AddNlogLogger();
            }
            
            return serviceCollection;
        }

        /// <summary>
        /// 添加Memory默认实现
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddMemoryDocument(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDocument();
            serviceCollection.TryAddSingleton<ISnapshotStore, MemorySnapshotStore>();
            serviceCollection.TryAddSingleton<IEventStore, MemoryEventStore>();
            return serviceCollection;
        }


        /// <summary>
        /// 添加内存形式的存储对象
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static StorageOptions AddDataPersistenceService(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton(typeof(IPersistenceService<>), typeof(DataPersistenceServiceAdapter<>));
            return serviceCollection.AddMemoryStorage();
        }

    }
}
