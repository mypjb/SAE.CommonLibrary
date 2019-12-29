﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SAE.CommonLibrary.ObjectMapper.Bindings;
using SAE.CommonLibrary.ObjectMapper.Core;
using SAE.CommonLibrary.ObjectMapper.Core.DataStructures;
using SAE.CommonLibrary.ObjectMapper.Core.Extensions;
using SAE.CommonLibrary.ObjectMapper.Mappers;
using SAE.CommonLibrary.ObjectMapper.Reflection;
[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("SAE.CommonLibrary.TinyMapper.Test")]
namespace SAE.CommonLibrary.ObjectMapper
{
    /// <summary>
    /// TinyMapper is an object to object mapper for .NET. The main advantage is performance.
    /// TinyMapper allows easily map object to object, i.e. properties or fields from one object to another.
    /// </summary>
    public static class TinyMapper
    {
        private static readonly ConcurrentDictionary<TypePair, Mapper> _mappers = new ConcurrentDictionary<TypePair, Mapper>();
        private static readonly TargetMapperBuilder _targetMapperBuilder;
        private static readonly TinyMapperConfig _config;

        static TinyMapper()
        {
            IDynamicAssembly assembly = DynamicAssemblyBuilder.Get();
            _targetMapperBuilder = new TargetMapperBuilder(assembly);
            _config = new TinyMapperConfig(_targetMapperBuilder);
        }

        /// <summary>
        /// Create a one-way mapping between Source and Target types.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TTarget">Target type.</typeparam>
        /// <remarks>The method is thread safe.</remarks>
        public static void Bind<TSource, TTarget>()
        {
            Bind(typeof(TSource), typeof(TTarget));
        }

        /// <summary>
        /// Create a one-way mapping between Source and Target types.
        /// </summary>
        /// <param name="sourceType">Source type.</param>
        /// <param name="targetType">Target type.</param>
        /// <remarks>The method is thread safe.</remarks>
        public static void Bind(Type sourceType, Type targetType)
        {
            if (sourceType == null)
            {
                throw new ArgumentNullException(nameof(sourceType));
            }
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }
            TypePair typePair = TypePair.Create(sourceType, targetType);

            var mapper = _targetMapperBuilder.Build(typePair);

            _mappers.AddOrUpdate(typePair, mapper, (a, b) => mapper);
        }

        /// <summary>
        /// Create a one-way mapping between Source and Target types.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TTarget">Target type.</typeparam>
        /// <param name="config">BindingConfig for custom binding.</param>
        /// <remarks>The method is thread safe.</remarks>
        public static void Bind<TSource, TTarget>(Action<IBindingConfig<TSource, TTarget>> config)
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();

            var bindingConfig = new BindingConfigOf<TSource, TTarget>();

            config(bindingConfig);

            var mapper = _targetMapperBuilder.Build(typePair, bindingConfig);

            _mappers.AddOrUpdate(typePair, mapper, (a, b) => mapper);
        }

        /// <summary>
        /// Find out if a binding exists from Source to Target.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TTarget">Target type.</typeparam>
        /// <returns>True if exists, otherwise - False.</returns>
        /// <remarks>The method is thread safe.</remarks>
        public static bool BindingExists<TSource, TTarget>()
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();
            return _mappers.ContainsKey(typePair);
        }

        /// <summary>
        /// Maps the source to Target type.
        /// The method can be called in parallel to Map methods, but cannot be called in parallel to Bind method.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TTarget">Target type.</typeparam>
        /// <param name="source">Source object.</param>
        /// <param name="target">Target object.</param>
        /// <returns>Mapped object.</returns>
        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target = default(TTarget))
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();

            Mapper mapper = GetMapper(typePair);
            var result = (TTarget)mapper.Map(source, target);

            return result;
        }

        /// <summary>
        /// Maps the source to Target type.
        /// The method can be called in parallel to Map methods, but cannot be called in parallel to Bind method.
        /// </summary>
        /// <param name="sourceType">Source type.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="source">Source object.</param>
        /// <param name="target">Target object.</param>
        /// <returns>Mapped object.</returns>
        public static object Map(Type sourceType, Type targetType, object source, object target = null)
        {
            TypePair typePair = TypePair.Create(sourceType, targetType);

            Mapper mapper = GetMapper(typePair);
            var result = mapper.Map(source, target);

            return result;
        }

        /// <summary>
        /// Configure the Mapper.
        /// </summary>
        /// <param name="config">Lambda to provide config settings</param>
        public static void Config(Action<ITinyMapperConfig> config)
        {
            config(_config);
        }

        /// <summary>
        /// Maps the source to Target type.
        /// The method can be called in parallel to Map methods, but cannot be called in parallel to Bind method.
        /// </summary>
        /// <typeparam name="TTarget">Target type.</typeparam>
        /// <param name="source">Source object [Not null].</param>
        /// <returns>Mapped object. The method can be called in parallel to Map methods, but cannot be called in parallel to Bind method.</returns>
        public static TTarget Map<TTarget>(object source)
        {
            if (source.IsNull())
            {
                throw Error.ArgumentNull("Source cannot be null. Use TinyMapper.Map<TSource, TTarget> method instead.");
            }

            TypePair typePair = TypePair.Create(source.GetType(), typeof(TTarget));

            Mapper mapper = GetMapper(typePair);
            var result = (TTarget)mapper.Map(source);

            return result;
        }

        [SuppressMessage("ReSharper", "All")]
        private static Mapper GetMapper(TypePair typePair)
        {
            return _mappers.GetOrAdd(typePair, _targetMapperBuilder.Build);
            
        }

        //Note: Lock should already be acquired for the mapper
//        private static Mapper GetPolymorphicMapping(TypePair types)
//        {
//            // Walk the polymorphic heirarchy until we find a mapping match
//            Type source = types.Source;
//
//            do
//            {
//                Mapper result;
//                foreach (Type iface in source.GetInterfaces())
//                {
//                    if (_mappers.TryGetValue(TypePair.Create(iface, types.Target), out result))
//                        return result;
//                }
//
//                if (_mappers.TryGetValue(TypePair.Create(source, types.Target), out result))
//                    return result;
//            }
//            while ((source = Helpers.BaseType(source)) != null);
//
//            return null;
//        }
    }
}
