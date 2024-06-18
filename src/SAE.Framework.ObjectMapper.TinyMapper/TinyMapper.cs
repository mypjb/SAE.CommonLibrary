using SAE.Framework.ObjectMapper.Bindings;
using SAE.Framework.ObjectMapper.Core;
using SAE.Framework.ObjectMapper.Core.DataStructures;
using SAE.Framework.ObjectMapper.Core.Extensions;
using SAE.Framework.ObjectMapper.Mappers;
using SAE.Framework.ObjectMapper.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SAE.Framework.ObjectMapper.Test")]

namespace SAE.Framework.ObjectMapper
{
    /// <summary>
    /// TinyMapper is an object to object mapper for .NET. The main advantage is performance.
    /// TinyMapper allows easily map object to object, i.e. properties or fields from one object to another.
    /// </summary>
    public class TinyMapper : IObjectMapper
    {
        private readonly Dictionary<TypePair, Mapper> _mappers;
        private readonly TargetMapperBuilder _targetMapperBuilder;
        private readonly TinyMapperConfig _config;
        private readonly object _lock;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="builders">构建器</param>
        public TinyMapper(IEnumerable<IObjectMapperBuilder> builders)
        {
            this._lock = new object();
            _mappers = new Dictionary<TypePair, Mapper>();
            IDynamicAssembly assembly = DynamicAssemblyBuilder.Get();
            _targetMapperBuilder = new TargetMapperBuilder(assembly);
            _config = new TinyMapperConfig(_targetMapperBuilder);
            foreach (var builder in builders)
            {
                builder.Build(this);
            }
        }

        /// <summary>
        /// Create a one-way mapping between Source and Target types.
        /// </summary>
        /// <param name="sourceType">Source type.</param>
        /// <param name="targetType">Target type.</param>
        /// <remarks>The method is thread safe.</remarks>
        public IObjectMapper Bind(Type sourceType, Type targetType)
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
            Bind(typePair, mapper);
            return this;
        }

        /// <summary>
        /// Create a one-way mapping between Source and Target types.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TTarget">Target type.</typeparam>
        /// <param name="config">BindingConfig for custom binding.</param>
        /// <remarks>The method is thread safe.</remarks>
        public IObjectMapper Bind<TSource, TTarget>(Action<IBindingConfig<TSource, TTarget>> config)
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();

            var bindingConfig = new BindingConfigOf<TSource, TTarget>();
            config(bindingConfig);
            var mapper = _targetMapperBuilder.Build(typePair, bindingConfig);
            Bind(typePair, mapper);
            return this;
        }

        private void Bind(TypePair typePair, Mapper mapper)
        {
            this._mappers[typePair] = mapper;//_mappers.AddOrUpdate(typePair, mapper, (key, oldMapper) => mapper);
        }

        /// <summary>
        /// Find out if a binding exists from Source to Target.
        /// </summary>
        /// <param name="sourceType">Source type.</param>
        /// <param name="targetType">Target type.</param>
        /// <returns>True if exists, otherwise - False.</returns>
        /// <remarks>The method is thread safe.</remarks>
        public bool BindingExists(Type sourceType, Type targetType)
        {
            TypePair typePair = TypePair.Create(sourceType, targetType);
            return _mappers.ContainsKey(typePair);
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
        public object Map(Type sourceType, Type targetType, object source, object target = null)
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
        public IObjectMapper Config(Action<ITinyMapperConfig> config)
        {
            config(_config);
            return this;
        }

        /// <summary>
        /// Maps the source to Target type.
        /// The method can be called in parallel to Map methods, but cannot be called in parallel to Bind method.
        /// </summary>
        /// <typeparam name="TTarget">Target type.</typeparam>
        /// <param name="source">Source object [Not null].</param>
        /// <returns>Mapped object. The method can be called in parallel to Map methods, but cannot be called in parallel to Bind method.</returns>
        public TTarget Map<TTarget>(object source)
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
        private Mapper GetMapper(TypePair typePair)
        {
            Mapper mapper;

            if (_mappers.TryGetValue(typePair, out mapper) == false)
            {
                lock (this._lock)
                {
                    if (_mappers.TryGetValue(typePair, out mapper) == false)
                    {
                        Bind(typePair.Source, typePair.Target);
                        mapper = GetMapper(typePair);
                    }
                }
            }
            return mapper;
        }
    }
}
