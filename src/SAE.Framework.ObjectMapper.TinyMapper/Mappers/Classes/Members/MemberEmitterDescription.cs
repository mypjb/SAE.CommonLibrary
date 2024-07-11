using System;
using SAE.Framework.ObjectMapper.CodeGenerators.Emitters;
using SAE.Framework.ObjectMapper.Core.DataStructures;
using SAE.Framework.ObjectMapper.Core.Extensions;
using SAE.Framework.ObjectMapper.Mappers.Caches;

namespace SAE.Framework.ObjectMapper.Mappers.Classes.Members
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
