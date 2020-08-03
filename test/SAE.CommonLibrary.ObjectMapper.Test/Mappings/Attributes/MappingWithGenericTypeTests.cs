using SAE.CommonLibrary.ObjectMapper;
using SAE.CommonLibrary.ObjectMapper.Bindings;
using Xunit;
using XAssert = Xunit.Assert;

namespace SAE.CommonLibrary.ObjectMapper.Test.Mappings.Attributes
{
    public sealed class MappingWithGenericTypeTests: MappingBase
    {
        interface IEntity<T>
        {
            T Key { get; set; }
        }


        public abstract class Entity<T> : IEntity<T>
        {
            public T Key { get; set; }
        }


        public class SourceDto
        {
            [Bind(nameof(Target.Key), typeof(Target))]
            public long Id { get; set; }
        }


        public class Target : Entity<long>
        {
        }


        private static SourceDto CreateSource()
        {
            return new SourceDto
            {
                Id = 23,
            };
        }

        private static Target CreateTarget()
        {
            return new Target
            {
                Key = 23,
            };
        }

        [Fact]
        public void Map_WithType_Success()
        {
            _tinyMapper.Bind<SourceDto, Target>();

            SourceDto source = CreateSource();
            var target = _tinyMapper.Map<Target>(source);

            XAssert.Equal(target.Key, source.Id);
        }

        [Fact]
        public void Map_WithType_Back_Success()
        {
            _tinyMapper.Bind<Target, SourceDto>();
            Target target = CreateTarget();
            var source = _tinyMapper.Map<SourceDto>(target);

            XAssert.Equal(source.Id, target.Key);
        }
    }
}