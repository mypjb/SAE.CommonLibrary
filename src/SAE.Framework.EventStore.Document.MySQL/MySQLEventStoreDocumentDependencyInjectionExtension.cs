using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.Framework;
using SAE.Framework.Database;
using SAE.Framework.EventStore;
using SAE.Framework.EventStore.Document.MySQL;
using SAE.Framework.EventStore.Snapshot;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="IEventStore"/> mysql注入
    /// </summary>
    public static class MySqlEventStoreDocumentDependencyInjectionExtension
    {
        /// <summary>
        /// 使用MySql存储文档
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <param name="assemblies">文档集合</param>
        /// <returns><paramref name="builder"/></returns>
        public static ISAEFrameworkBuilder AddMySQLDocument(this ISAEFrameworkBuilder builder, params Assembly[] assemblies)
        {
            if (assemblies == null || !assemblies.Any())
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }
            var services = builder.Services;

            builder.AddDocument(assemblies)
                   .AddMySQLDatabase();
            services.TryAddSingleton<ISnapshotStore, MySQLSnapshotStore>();
            services.TryAddSingleton<IEventStore, MySqlEventStore>();

            return builder;
        }
    }
}
