using System;
using SAE.Framework.ObjectMapper;
using Xunit;

namespace SAE.Framework.ObjectMapper.Test
{
    public sealed class ForTests : MappingBase
    {

        public ForTests()
        {
        }
        [Fact]
        public void Test()
        {
            var source = new TestStaticModel();

            //TinyMapper.Bind<TestStaticModel, TestDto>();
            var dto = Map<TestStaticModel, TestDto>(source);
            // OR

        }

        private TTarget Map<TSource, TTarget>(TSource source)
        {
            //if (!TinyMapper.BindingExists<TSource, TTarget>())
            //{
            //    TinyMapper.Bind<TSource, TTarget>();
            //}
            return _tinyMapper.Map<TTarget>(source);
        }
    }


    public class TestStaticModel
    {
        public static string Name = "test";
        public static int Id = 1;
    }


    public class TestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
