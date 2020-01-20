using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Data;
using SAE.CommonLibrary.Data.Memory;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// 添加内存Storage,只限于测试使用
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static StorageOptions AddMemoryStorage(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IMetadataProvider, MetadataProvider>();
            serviceCollection.TryAddSingleton<IStorage, MemoryStorage>();
            serviceCollection.AddNlogLogger();
            return new StorageOptions(serviceCollection);
        }

        public static StorageOptions AddMapper<T, TDto>(this StorageOptions options) where T : class
                                                                                     where TDto : class
        {
            var metadata = new Metadata<T>();
            var dtoMetadata = new Metadata<TDto>(metadata.Name);
            options.ServiceCollection.TryAddSingleton(metadata);
            options.ServiceCollection.TryAddSingleton(dtoMetadata);
            return options;
        }

        public static StorageOptions AddMapper<T>(this StorageOptions options, string name) where T : class
        {
            var metadata = new Metadata<T>(name);
            options.ServiceCollection.TryAddSingleton(metadata);
            return options;
        }

        public static StorageOptions AddMapper<T>(this StorageOptions options, string name, Func<T, object> identityFactory) where T : class
        {
            var metadata = new Metadata<T>(name, identityFactory);
            options.ServiceCollection.TryAddSingleton(metadata);
            return options;
        }
    }
}