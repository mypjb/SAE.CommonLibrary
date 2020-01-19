using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Data;
using SAE.CommonLibrary.Data.MongoDB;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// 添加Storage
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="config">mongodb配置</param>
        /// <returns></returns>
        public static MongoDBOptions AddMongoDB(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IDescriptionProvider, DescriptionProvider>();
            serviceCollection.TryAddSingleton<IStorage, MongoDBStorage>();
            serviceCollection.AddNlogLogger();
            serviceCollection.AddSaeOptions<MongoDBConfig>("mongodb");
            return new MongoDBOptions(serviceCollection);
        }


        public static MongoDBOptions AddMapping<TTable>(this MongoDBOptions options, string table) where TTable : class
        {
            options.ServiceCollection.AddSingleton(new TableDescription<TTable>(table, s =>
            {
                return ((dynamic)s).Id;
            }));
            return options;
        }
    }
}