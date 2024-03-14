using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using SAE.CommonLibrary.Data;
using SAE.CommonLibrary.Data.MongoDB;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="IStorage"/>Mongo配置类
    /// </summary>
    public static class MongoDBDependencyInjectionExtension
    {
        /// <summary>
        /// 注册<see cref="IStorage"/>mongo实现
        /// </summary>
        /// <param name="serviceCollection">服务集合</param>
        /// <returns>构建器</returns>
        public static StorageBuilder AddMongoDB(this IServiceCollection serviceCollection)
        {
            var conventions = new ConventionPack
                        {
                             new IgnoreExtraElementsConvention(true)
                        };
            var conventionName = "IgnoreExtraElements";

            ConventionRegistry.Remove(conventionName);

            ConventionRegistry.Register(conventionName, conventions, type => true);

            serviceCollection.TryAddSingleton<IMetadataProvider, DefaultMetadataProvider>();
            serviceCollection.TryAddSingleton<IStorage, MongoDBStorage>();
            serviceCollection.AddDefaultLogger();
            serviceCollection.AddOptions<MongoDBOptions>()
                             .Bind(MongoDBOptions.Option);
            return new StorageBuilder(serviceCollection);
        }

    }
}