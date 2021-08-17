using System;
using SAE.CommonLibrary.ObjectMapper.Bindings;
using SAE.CommonLibrary.ObjectMapper.Core.DataStructures;
using SAE.CommonLibrary.ObjectMapper.Core.Extensions;
using SAE.CommonLibrary.ObjectMapper.Mappers;
using SAE.CommonLibrary.ObjectMapper.Mappers.Classes.Members;
using SAE.CommonLibrary.ObjectMapper.Reflection;

namespace SAE.CommonLibrary.ObjectMapper.Test
{
    internal class MappingBuilderConfigStub : IMapperBuilderConfig
    {
        private readonly Option<BindingConfig> _bindingConfig = Option<BindingConfig>.Empty;

        public MappingBuilderConfigStub()
        {
        }

        public MappingBuilderConfigStub(BindingConfig bindingConfig)
        {
            _bindingConfig = bindingConfig.ToOption();
        }

        public IDynamicAssembly Assembly => DynamicAssemblyBuilder.Get();

        public Func<string, string, bool> NameMatching => TargetMapperBuilder.DefaultNameMatching;

        public Option<BindingConfig> GetBindingConfig(TypePair typePair)
        {
            return _bindingConfig;
        }

        public MapperBuilder GetMapperBuilder(TypePair typePair)
        {
            throw new NotImplementedException();
        }

        public MapperBuilder GetMapperBuilder(TypePair parentTypePair, MappingMember mappingMember)
        {
            throw new NotImplementedException();
        }
    }
}
