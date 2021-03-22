using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson.Serialization.Conventions;
using SAE.CommonLibrary.Data;
using SAE.CommonLibrary.Data.MongoDB;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class MongoDBDependencyInjectionExtension
    {
        /// <summary>
        /// 添加Storage
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="config">mongodb配置</param>
        /// <returns></returns>
        public static StorageOptions AddMongoDB(this IServiceCollection serviceCollection)
        {
            var conventions = new ConventionPack
                        {
                             new IgnoreExtraElementsConvention(true)
                        };
            var conventionName = "IgnoreExtraElements";

            ConventionRegistry.Remove(conventionName);

            ConventionRegistry.Register(conventionName, conventions, type => true);

            serviceCollection.TryAddSingleton<IMetadataProvider, MetadataProvider>();
            serviceCollection.TryAddSingleton<IStorage, MongoDBStorage>();
            serviceCollection.AddNlogLogger();
            serviceCollection.AddOptions<MongoDBOptions>()
                             .Bind(MongoDBOptions.Option);
            return new StorageOptions(serviceCollection);
        }

    }
}