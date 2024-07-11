using System;
using System.Collections.Generic;
using System.Linq;
using SAE.Framework.ObjectMapper;
using Xunit;
using XAssert = Xunit.Assert;

namespace SAE.Framework.ObjectMapper.Test.Mappings.Collections
{
    public sealed class DictionaryMappingTests : MappingBase
    {
        [Fact]
        public void Map_Dictionary_Success()
        {
            _tinyMapper.Bind<Source1, Target1>();

            var source = new Source1
            {
                Id = Guid.NewGuid(),
                Dictionary = new Dictionary<string, int> { { "Key1", 1 }, { "Key2", 2 } }
            };

            var target = _tinyMapper.Map<Target1>(source);

            XAssert.Equal(source.Id, target.Id);
            XAssert.Equal(source.Dictionary, target.Dictionary);
        }

        [Fact]
        public void Map_IDictionary_Success()
        {
            _tinyMapper.Bind<Source3, Target3>();

            var source = new Source3
            {
                Id = Guid.NewGuid(),
                Dictionary = new Dictionary<string, int> { { "Key1", 1 }, { "Key2", 2 } }
            };

            var target = _tinyMapper.Map<Target3>(source);

            XAssert.Equal(source.Id, target.Id);
            XAssert.Equal(source.Dictionary, target.Dictionary);
        }

        [Fact]
        public void Map_IDictionary_Target_Success()
        {
            _tinyMapper.Bind<Source1, Target3>();

            var source = new Source1
            {
                Id = Guid.NewGuid(),
                Dictionary = new Dictionary<string, int> { { "Key1", 1 }, { "Key2", 2 } }
            };

            var target = _tinyMapper.Map<Target3>(source);

            XAssert.Equal(source.Id, target.Id);
            XAssert.Equal(source.Dictionary, target.Dictionary);
        }

        [Fact]
        public void Map_DifferentKeyDictionary_Success()
        {
            _tinyMapper.Bind<ItemKeySource, ItemKeyTarget>();
            _tinyMapper.Bind<Source2, Target2>();

            var source = new Source2
            {
                Id = Guid.NewGuid(),
                Dictionary = new Dictionary<ItemKeySource, int>
                {
                    { new ItemKeySource { Id = Guid.NewGuid() }, 1 },
                    { new ItemKeySource { Id = Guid.NewGuid() }, 2 },
                }
            };

            var target = _tinyMapper.Map<Target2>(source);

            XAssert.Equal(source.Id, target.Id);
            XAssert.Equal(source.Dictionary.Keys.Select(x => x.Id), target.Dictionary.Keys.Select(x => x.Id));
            XAssert.Equal(source.Dictionary.Values, target.Dictionary.Values);
        }

        public class ItemKeySource
        {
            public Guid Id { get; set; }
        }

        public class ItemKeyTarget
        {
            public Guid Id { get; set; }
        }

        public class Source1
        {
            public Dictionary<string, int> Dictionary { get; set; }
            public Guid Id { get; set; }
        }

        public class Source2
        {
            public Dictionary<ItemKeySource, int> Dictionary { get; set; }
            public Guid Id { get; set; }
        }

        public class Source3
        {
            public IDictionary<string, int> Dictionary { get; set; }
            public Guid Id { get; set; }
        }

        public class Target1
        {
            public Dictionary<string, int> Dictionary { get; set; }
            public Guid Id { get; set; }
        }

        public class Target2
        {
            public Dictionary<ItemKeyTarget, int> Dictionary { get; set; }
            public Guid Id { get; set; }
        }

        public class Target3
        {
            public IDictionary<string, int> Dictionary { get; set; }
            public Guid Id { get; set; }
        }
    }
}
