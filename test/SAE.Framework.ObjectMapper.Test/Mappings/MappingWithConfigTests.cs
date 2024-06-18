using System;
using System.Collections.Generic;
using SAE.Framework.ObjectMapper;
using Xunit;
using XAssert = Xunit.Assert;
namespace SAE.Framework.ObjectMapper.Test.Mappings
{
    public sealed class MappingWithConfigTests : MappingBase
    {
        [Fact]
        public void Map_Bind_Success()
        {
            _tinyMapper.Bind<SourceItem, TargetItem>();

            _tinyMapper.Bind<Source1, Target1>(config =>
            {
                config.Bind(from => from.String, to => to.MyString);
                config.Bind(from => from.Int, to => to.MyInt);
                config.Bind(from => from.SourceItem, to => to.TargetItem);
            });

            var source = new Source1
            {
                Bool = true,
                Byte = 5,
                Int = 9,
                String = "Test",
                DateTime = DateTime.Now,
                SourceItem = new SourceItem { Id = Guid.NewGuid() }
            };

            var actual = _tinyMapper.Map<Target1>(source);

            XAssert.Equal(source.Bool, actual.Bool);
            XAssert.Equal(source.String, actual.MyString);
            XAssert.Equal(source.Byte, actual.Byte);
            XAssert.Equal(source.Int, actual.MyInt);
            XAssert.Equal(source.DateTime, actual.DateTime);
            XAssert.Equal(source.SourceItem.Id, actual.TargetItem.Id);
        }

        [Fact]
        public void Map_BindCaseSensitive_Success()
        {
            _tinyMapper.Bind<SourceItem, TargetItem>();

            _tinyMapper.Bind<Source1, Target1>(config =>
            {
                config.Bind(from => from.CaseSensitive, to => to.Casesensitive);
            });

            var source = new Source1
            {
                CaseSensitive = "CaseSensitive"
            };

            var actual = _tinyMapper.Map<Target1>(source);

            XAssert.Equal(source.CaseSensitive, actual.Casesensitive);
        }

        [Fact]
        public void Map_ConcreteType_Success()
        {
            _tinyMapper.Bind<Source2, Target2>(config =>
            {
                config.Bind(target => target.Ints, typeof(List<int>));
                config.Bind(target => target.Strings, typeof(List<string>));
            });

            var source = new Source2
            {
                Ints = new List<int> { 1, 2, 3 },
                Strings = new List<string> { "A", "B", "C" }
            };

            var actual = _tinyMapper.Map<Target2>(source);
            XAssert.Equal(source.Ints, actual.Ints);
            XAssert.Equal(source.Strings, actual.Strings);
        }

        [Fact]
        public void Map_Ignore_Success()
        {
            _tinyMapper.Bind<Source1, Target1>(config =>
            {
                config.Ignore(x => x.Bool);
                config.Ignore(x => x.String);
            });

            var source = new Source1
            {
                Bool = true,
                Byte = 5,
                Int = 9,
                String = "Test",
            };

            var actual = _tinyMapper.Map<Target1>(source);

            XAssert.False(actual.Bool);
            XAssert.Null(actual.MyString);
            XAssert.Equal(source.Byte, actual.Byte);
            XAssert.Equal(0, actual.MyInt);
            XAssert.Null(actual.TargetItem);
        }
        
        [Fact]
        public void Map_MultiTarget()
        {
            _tinyMapper.Bind<SourceName, TargetName>(config =>
            {
                config.Bind(s => s.CName, t => t.C2Name1);
                config.Bind(s => s.CName, t => t.C2Name2);
            });

            var result = _tinyMapper.Map<TargetName>(new SourceName
            {
                CName = "7788"
            });

            XAssert.Equal("7788", result.C2Name1);
            XAssert.Equal("7788", result.C2Name2);
        }
        
        public class SourceName
        {
            public string CName { get; set; }
        }
    
        public class TargetName
        {
            public string C2Name1 { get; set; }
            public string C2Name2 { get; set; }
        }

        public class Source1
        {
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public int Int { get; set; }
            public DateTime? DateTime { get; set; }
            public string CaseSensitive { get; set; }

            public SourceItem SourceItem { get; set; }
            public string String { get; set; }
        }

        public class Source2
        {
            public IList<int> Ints { get; set; }
            public List<string> Strings { get; set; }
        }

        public class SourceItem
        {
            public Guid Id { get; set; }
        }

        public class Target1
        {
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public int MyInt { get; set; }
            public DateTime? DateTime { get; set; }
            public string MyString { get; set; }
            public string Casesensitive { get; set; }

            public TargetItem TargetItem { get; set; }
        }

        public class Target2
        {
            public IList<int> Ints { get; set; }
            public IEnumerable<string> Strings { get; set; }
        }

        public class TargetItem
        {
            public Guid Id { get; set; }
        }
    }
}
