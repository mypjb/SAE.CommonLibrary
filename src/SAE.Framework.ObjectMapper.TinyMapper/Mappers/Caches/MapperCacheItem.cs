﻿using System;
using System.Reflection;
using SAE.Framework.ObjectMapper.CodeGenerators.Emitters;

namespace SAE.Framework.ObjectMapper.Mappers.Caches
{
    internal sealed class MapperCacheItem
    {
        public int Id { get; set; }
        public Mapper Mapper { get; set; }

        public IEmitterType EmitMapMethod(IEmitterType sourceMemeber, IEmitterType targetMember)
        {
            Type mapperType = typeof(Mapper);
            MethodInfo mapMethod = mapperType.GetMethod(Mapper.MapMethodName, BindingFlags.Instance | BindingFlags.Public);
            FieldInfo mappersField = mapperType.GetField(Mapper.MappersFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            IEmitterType mappers = EmitField.Load(EmitThis.Load(mapperType), mappersField);
            IEmitterType mapper = EmitArray.Load(mappers, Id);
            IEmitterType result = EmitMethod.Call(mapMethod, mapper, sourceMemeber, targetMember);
            return result;
        }
    }
}
