using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Database;
using SAE.CommonLibrary.EventStore.Snapshot;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore.Document.MySql
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
