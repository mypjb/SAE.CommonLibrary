using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Database;
using SAE.CommonLibrary.EventStore;
using SAE.CommonLibrary.EventStore.Document.MySql;
using SAE.CommonLibrary.EventStore.Snapshot;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MySqlEventStoreDocumentDependencyInjectionExtension
    {
        /// <summary>
        /// Add MySql Document Store
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddMySqlDocument(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDocument()
                             .AddMySqlDatabase();
            serviceCollection.TryAddSingleton<ISnapshotStore, MySqlSnapshotStore>();
            serviceCollection.TryAddSingleton<IEventStore, MySqlEventStore>();
            return serviceCollection;
        }
    }
}
