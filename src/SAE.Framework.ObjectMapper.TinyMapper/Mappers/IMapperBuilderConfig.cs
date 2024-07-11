using System;
using SAE.Framework.ObjectMapper.Bindings;
using SAE.Framework.ObjectMapper.Core.DataStructures;
using SAE.Framework.ObjectMapper.Mappers.Classes.Members;
using SAE.Framework.ObjectMapper.Reflection;

namespace SAE.Framework.ObjectMapper.Mappers
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
