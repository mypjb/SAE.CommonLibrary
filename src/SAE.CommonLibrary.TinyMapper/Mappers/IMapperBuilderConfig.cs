using System;
using SAE.CommonLibrary.ObjectMapper.Bindings;
using SAE.CommonLibrary.ObjectMapper.Core.DataStructures;
using SAE.CommonLibrary.ObjectMapper.Mappers.Classes.Members;
using SAE.CommonLibrary.ObjectMapper.Reflection;

namespace SAE.CommonLibrary.ObjectMapper.Mappers
{
    internal interface IMapperBuilderConfig
    {
        IDynamicAssembly Assembly { get; }
        Func<string, string, bool> NameMatching { get; }
        Option<BindingConfig> GetBindingConfig(TypePair typePair);
        MapperBuilder GetMapperBuilder(TypePair typePair);
        MapperBuilder GetMapperBuilder(TypePair parentTypePair, MappingMember mappingMember);
    }
}
