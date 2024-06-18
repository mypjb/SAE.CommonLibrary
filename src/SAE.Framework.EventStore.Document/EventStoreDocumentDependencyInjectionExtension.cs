using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.Framework;
using SAE.Framework.Data;
using SAE.Framework.EventStore;
using SAE.Framework.EventStore.Document;
using SAE.Framework.EventStore.Document.Memory;
using SAE.Framework.EventStore.Serialize;
using SAE.Framework.EventStore.Snapshot;
using SAE.Framework.Extension;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 事件存储注册类
    /// </summary>
    public static class EventStoreDocumentDependencyInjectionExtension
    {
        /// <summary>
        /// 添加EventStore.Docment默认实现
        /// </summary>
        /// <param name="builder">服务接口</param>
        /// <param name="assemblies">文档所在的程序集集合</param>
        /// <returns><paramref name="builder"/></returns>
        public static ISAEFrameworkBuilder AddDocument(this ISAEFrameworkBuilder builder, params Assembly[] assemblies)
        {
            var services = builder.Services;

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
                builder.AddDefaultLogger();
            }

            services.TryAddSingleton<ISerializer,DefaultSerializer>();

            return builder;
        }

        /// <summary>
        /// 添加Memory默认实现
        /// </summary>
        /// <param name="builder">服务接口</param>
        /// <param name="assemblies"></param>
        /// <returns><paramref name="builder"/></returns>
        public static ISAEFrameworkBuilder AddMemoryDocument(this ISAEFrameworkBuilder builder, params Assembly[] assemblies)
        {
            var services = builder.Services;
            if (assemblies == null || assemblies.Count() == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }
            builder.AddDocument(assemblies);

            services.TryAddSingleton<ISnapshotStore, MemorySnapshotStore>();
            services.TryAddSingleton<IEventStore, MemoryEventStore>();

            return builder;
        }


        /// <summary>
        /// 添加内存形式的存储对象
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static StorageBuilder AddMemoryDataPersistenceService(this ISAEFrameworkBuilder builder)
        {
            var services = builder.Services;
            services.TryAddSingleton(typeof(IPersistenceService<>), typeof(DefaultDataPersistenceServiceAdapter<>));
            return builder.AddMemoryStorage();
        }
        /// <summary>
        /// 添加基于内存的持久化数据存储
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        public static ISAEFrameworkBuilder AddDataPersistenceService(this ISAEFrameworkBuilder builder, Action<StorageBuilder> configure)
        {
            configure.Invoke(builder.AddMemoryDataPersistenceService());
            return builder;
        }
        /// <summary>
        /// 添加默认的持久化存储
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies"></param>
        public static ISAEFrameworkBuilder AddDataPersistenceService(this ISAEFrameworkBuilder builder, params Assembly[] assemblies)
        {
            if (assemblies == null || !assemblies.Any())
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            builder.AddDataPersistenceService(option =>
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
            builder.AddTinyMapper();
            return builder;
        }


    }
}
