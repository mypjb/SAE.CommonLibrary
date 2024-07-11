using System;
using System.Reflection;
using SAE.Framework.ObjectMapper.Core.DataStructures;

namespace SAE.Framework.ObjectMapper.Mappers.Classes.Members
{
    internal sealed class MappingMember
    {
        public MappingMember(MemberInfo source, MemberInfo target, TypePair typePair)
        {
            Source = source;
            Target = target;
            TypePair = typePair;
        }

        public MemberInfo Source { get; }
        public MemberInfo Target { get; }
        public TypePair TypePair { get; }
    }
}
