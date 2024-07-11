using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.Framework;
using SAE.Framework.Data;
using SAE.Framework.Data.Memory;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="IStorage"/>注册
    /// </summary>
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// 添加内存Storage,只限于测试使用
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <returns>服务集合</returns>
        public static StorageBuilder AddMemoryStorage(this ISAEFrameworkBuilder builder)
        {
            var services = builder.Services;
            services.TryAddSingleton<IMetadataProvider, DefaultMetadataProvider>();
            services.TryAddSingleton<IStorage, MemoryStorage>();
            builder.AddDefaultLogger()
                   .AddTinyMapper();
            return new StorageBuilder(services);
        }
        /// <summary>
        /// 添加映射
        /// </summary>
        /// <typeparam name="T">根类型</typeparam>
        /// <typeparam name="TDto">传输类型</typeparam>
        /// <param name="options"><see cref="IStorage"/>配置</param>
        /// <returns><see cref="IStorage"/>配置</returns>
        public static StorageBuilder AddMapper<T, TDto>(this StorageBuilder options) where T : class
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
        public static StorageBuilder AddMapper<T>(this StorageBuilder options, string name) where T : class
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
        public static StorageBuilder AddMapper<T>(this StorageBuilder options, string name, Func<T, object> identityFactory) where T : class
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
        public static StorageBuilder AddMapper(this StorageBuilder options, Type documentType, Type dtoType)
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