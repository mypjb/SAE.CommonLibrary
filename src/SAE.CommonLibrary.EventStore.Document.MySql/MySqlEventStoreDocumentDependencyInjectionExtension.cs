using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Database;
using SAE.CommonLibrary.EventStore;
using SAE.CommonLibrary.EventStore.Document.MySql;
using SAE.CommonLibrary.EventStore.Snapshot;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MySqlEventStoreDocumentDependencyInjectionExtension
    {
        /// <summary>
        /// Add MySql Document Store
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddMySqlDocument(this IServiceCollection serviceCollection, params Assembly[] assemblies)
        {
            if (assemblies == null || !assemblies.Any())
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            serviceCollection.AddDocument(assemblies)
                             .AddMySqlDatabase();
            serviceCollection.TryAddSingleton<ISnapshotStore, MySqlSnapshotStore>();
            serviceCollection.TryAddSingleton<IEventStore, MySqlEventStore>();
            return serviceCollection;
        }
    }
}
