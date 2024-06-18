using System;
using SAE.Framework.ObjectMapper.Bindings;
using SAE.Framework.ObjectMapper.Core.DataStructures;
using SAE.Framework.ObjectMapper.Core.Extensions;
using SAE.Framework.ObjectMapper.Mappers;
using SAE.Framework.ObjectMapper.Mappers.Classes.Members;
using SAE.Framework.ObjectMapper.Reflection;

namespace SAE.Framework.ObjectMapper.Test
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
