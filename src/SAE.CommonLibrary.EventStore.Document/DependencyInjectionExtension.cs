using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Data;
using SAE.CommonLibrary.EventStore;
using SAE.CommonLibrary.EventStore.Document;
using SAE.CommonLibrary.EventStore.Document.Memory;
using SAE.CommonLibrary.EventStore.Snapshot;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
                serviceCollection.AddSaeOptions<DocumentOptions>();
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
        public static StorageOptions AddMemoryDataPersistenceService(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton(typeof(IPersistenceService<>), typeof(DataPersistenceServiceAdapter<>));
            return serviceCollection.AddMemoryStorage();
        }

        public static IServiceCollection AddDataPersistenceService(this IServiceCollection serviceCollection, Action<StorageOptions> configure)
        {
            configure.Invoke(serviceCollection.AddMemoryDataPersistenceService());
            return serviceCollection;
        }

        public static IServiceCollection AddDataPersistenceService(this IServiceCollection serviceCollection, params Assembly[] assemblies)
        {
            if (assemblies == null || !assemblies.Any())
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            serviceCollection.AddDataPersistenceService(option =>
            {
                var documentType = typeof(IDocument);

                var documentTypes = new List<Type>();

                var types = new List<Type>();

                assemblies.ForEach(s =>
                {
                    documentTypes.AddRange(s.GetTypes()
                                 .Where(t => t.IsPublic &&
                                        !t.IsAbstract &&
                                        documentType.IsAssignableFrom(t)));

                    types.AddRange(s.GetTypes());
                });



                documentTypes.ForEach(s =>
                {
                    var dtoType = types.FirstOrDefault(t => t.Name.Equals($"{s.Name}Dto", StringComparison.OrdinalIgnoreCase));
                    if(dtoType==null)return;
                    option.AddMapper(s, dtoType);
                });
            });
            return serviceCollection;
        }


    }
}
