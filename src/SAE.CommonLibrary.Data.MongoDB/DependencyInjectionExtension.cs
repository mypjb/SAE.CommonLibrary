using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Data;
using SAE.CommonLibrary.Data.MongoDB;

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
        public static IServiceCollection AddMongoDB(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IStorage, MongoDBStorage>();
            serviceCollection.AddNlogLogger();
            serviceCollection.AddSaeOptions<MongoDBConfig>("mongodb");
            return serviceCollection;
        }

        
    }
}