using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using SAE.Framework;
using SAE.Framework.Data;
using SAE.Framework.Data.MongoDB;

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
        /// <param name="builder">服务集合</param>
        /// <returns>构建器</returns>
        public static StorageBuilder AddMongoDB(this ISAEFrameworkBuilder builder)
        {
            var services = builder.Services;

            var conventions = new ConventionPack
                        {
                             new IgnoreExtraElementsConvention(true)
                        };
            var conventionName = "IgnoreExtraElements";

            ConventionRegistry.Remove(conventionName);

            ConventionRegistry.Register(conventionName, conventions, type => true);

            services.TryAddSingleton<IMetadataProvider, DefaultMetadataProvider>();
            services.TryAddSingleton<IStorage, MongoDBStorage>();
            builder.AddDefaultLogger();
            services.AddOptions<MongoDBOptions>()
                    .Bind(MongoDBOptions.Option);
            return new StorageBuilder(services);
        }

    }
}