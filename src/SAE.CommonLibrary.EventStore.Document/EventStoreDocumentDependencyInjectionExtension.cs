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
    public static class EventStoreDocumentDependencyInjectionExtension
    {
        /// <summary>
        /// 添加EventStore.Docment默认实现
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDocument(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Count() == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            var types = new List<Type>();

            foreach (var assembly in assemblies)
            {
                var eventInterface = typeof(IEvent);
                foreach (var type in assembly.GetAssignableFrom(eventInterface)
                                             .Where(t => !t.IsInterface &&
                                                         !t.IsAbstract &&
                                                         t.IsClass))
                {
                    types.Add(type);
                }
            }


            var provider = new EventMappingProvider(types);

            services.AddSingleton(provider);

            services.TryAddSingleton<IEventMapping, DefaultEventMapping>();

            if (!services.IsRegister<IDocumentStore>())
            {
                services.AddOptions<DocumentOptions>()
                        .Bind(DocumentOptions.Option);

                services.TryAddSingleton<IDocumentStore, DefaultDocumentStore>();
                services.TryAddSingleton<IDocumentEvent, DefaultDocumentEvent>();
                services.AddNlogLogger();
            }

            return services;
        }

        /// <summary>
        /// 添加Memory默认实现
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddMemoryDocument(this IServiceCollection serviceCollection, params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Count() == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }
            serviceCollection.AddDocument(assemblies);
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
                    if (dtoType == null) return;
                    option.AddMapper(s, dtoType);
                });
            });
            serviceCollection.AddTinyMapper();
            return serviceCollection;
        }


    }
}
