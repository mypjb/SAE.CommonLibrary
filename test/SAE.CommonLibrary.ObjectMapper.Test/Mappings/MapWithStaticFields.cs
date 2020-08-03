using SAE.CommonLibrary.ObjectMapper;
using Xunit;
using XAssert = Xunit.Assert;

namespace SAE.CommonLibrary.ObjectMapper.Test.Mappings
{
    public class MapWithStaticFields : MappingBase
    {
        [Fact]
        public void MapStaticFields()
        {
            var source = new SourceStatic();

            _tinyMapper.Bind<SourceStatic, TargetDto>();
            var actual = _tinyMapper.Map<TargetDto>(source);

            XAssert.Equal(SourceStatic.Id, actual.Id);
            XAssert.Equal(SourceStatic.Name, actual.Name);
        }
    }

    public class SourceStatic
    {
        public static string Name = "test";
        public static int Id = 1;
    }


    public class TargetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
