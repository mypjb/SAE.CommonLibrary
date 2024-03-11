using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Data;
using SAE.CommonLibrary.Data.Memory;
using System;
using System.Reflection;

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
        /// <param name="serviceCollection">服务集合</param>
        /// <returns>服务集合</returns>
        public static StorageOptions AddMemoryStorage(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IMetadataProvider, MetadataProvider>();
            serviceCollection.TryAddSingleton<IStorage, MemoryStorage>();
            serviceCollection.AddDefaultLogger()
                             .AddTinyMapper();
            return new StorageOptions(serviceCollection);
        }
        /// <summary>
        /// 添加映射
        /// </summary>
        /// <typeparam name="T">根类型</typeparam>
        /// <typeparam name="TDto">传输类型</typeparam>
        /// <param name="options"><see cref="IStorage"/>配置</param>
        /// <returns><see cref="IStorage"/>配置</returns>
        public static StorageOptions AddMapper<T, TDto>(this StorageOptions options) where T : class
                                                                                     where TDto : class
        {
            var metadata = new Metadata<T>();
            var dtoMetadata = new Metadata<TDto>(metadata.Name);
            options.ServiceCollection.TryAddSingleton(metadata);
            options.ServiceCollection.TryAddSingleton(dtoMetadata);
            return options;
        }
        /// <summary>
        /// 添加映射
        /// </summary>
        /// <typeparam name="T">根类型</typeparam>
        /// <param name="options"><see cref="IStorage"/>配置</param>
        /// <param name="name">元数据名称</param>
        /// <returns><see cref="IStorage"/>配置</returns>
        public static StorageOptions AddMapper<T>(this StorageOptions options, string name) where T : class
        {
            var metadata = new Metadata<T>(name);
            options.ServiceCollection.TryAddSingleton(metadata);
            return options;
        }
        /// <summary>
        /// 添加映射
        /// </summary>
        /// <typeparam name="T">根类型</typeparam>
        /// <param name="options"><see cref="IStorage"/>配置</param>
        /// <param name="name">元数据名称</param>
        /// <param name="identityFactory">标识工厂</param>
        /// <returns><see cref="IStorage"/>配置</returns>
        public static StorageOptions AddMapper<T>(this StorageOptions options, string name, Func<T, object> identityFactory) where T : class
        {
            var metadata = new Metadata<T>(name, identityFactory);
            options.ServiceCollection.TryAddSingleton(metadata);
            return options;
        }
        /// <summary>
        /// 添加映射
        /// </summary>
        /// <param name="options"><see cref="IStorage"/>配置</param>
        /// <param name="documentType">文档类型</param>
        /// <param name="dtoType">传输类型</param>
        /// <returns><see cref="IStorage"/>配置</returns>
        public static StorageOptions AddMapper(this StorageOptions options, Type documentType, Type dtoType)
        {
            var metadataType = typeof(Metadata<>);
            var matadataDocumentType = metadataType.MakeGenericType(documentType);
            var matadataDtoType = metadataType.MakeGenericType(dtoType);
            var doctument = matadataDocumentType.GetConstructor(Type.EmptyTypes).Invoke(null);
            var dto = matadataDtoType.GetConstructor(new[] { typeof(string) }).Invoke(new[] { documentType.Name });
            options.ServiceCollection.TryAddSingleton(matadataDocumentType, s => doctument);
            options.ServiceCollection.TryAddSingleton(matadataDtoType, s => dto);
            return options;
        }
    }
}