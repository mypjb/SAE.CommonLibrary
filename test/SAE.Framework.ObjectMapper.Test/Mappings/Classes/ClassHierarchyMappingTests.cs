using System;
using SAE.Framework.ObjectMapper;
using Xunit;
using XAssert = Xunit.Assert;

namespace SAE.Framework.ObjectMapper.Test.Mappings.Classes
{
    public sealed class ClassHierarchyMappingTests : MappingBase
    {
        [Fact]
        public void Map_Hierarchy_Success()
        {
            _tinyMapper.Bind<Source, Target>();

            var source = new Source
            {
                Id = 1,
                String = "tiny"
            };

            var actual = _tinyMapper.Map<Target>(source);

            XAssert.Equal(source.Id, actual.Id);
            XAssert.Equal(source.String, actual.String);
        }


        public class Source : SourceBase
        {
            public string String { get; set; }
        }


        public abstract class SourceBase
        {
            public int Id { get; set; }
        }


        public abstract class TargetBase
        {
            public int Id { get; set; }
        }


        public sealed class Target : TargetBase
        {
            public string String { get; set; }
        }
    }
}
