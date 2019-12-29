using System;
using SAE.CommonLibrary.ObjectMapper.CodeGenerators.Emitters;
using SAE.CommonLibrary.ObjectMapper.Core.DataStructures;
using SAE.CommonLibrary.ObjectMapper.Core.Extensions;
using SAE.CommonLibrary.ObjectMapper.Mappers.Caches;

namespace SAE.CommonLibrary.ObjectMapper.Mappers.Classes.Members
{
    internal sealed class MemberEmitterDescription
    {
        public MemberEmitterDescription(IEmitter emitter, MapperCache mappers)
        {
            Emitter = emitter;
            MapperCache = new Option<MapperCache>(mappers, mappers.IsEmpty);
        }

        public IEmitter Emitter { get; }
        public Option<MapperCache> MapperCache { get; private set; }

        public void AddMapper(MapperCache value)
        {
            MapperCache = value.ToOption();
        }
    }
}
