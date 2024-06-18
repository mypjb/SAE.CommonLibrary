using System;
using System.Collections.Generic;
using SAE.Framework.ObjectMapper;
using Xunit;
using XAssert = Xunit.Assert;

namespace SAE.Framework.ObjectMapper.Test.Mappings.Collections
{
    public sealed class PrimitiveCollectionMappingTests : MappingBase
    {
        [Fact]
        public void Map_Arrays_Success()
        {
            _tinyMapper.Bind<Source1, Target1>();

            var source = new Source1
            {
                Ints = new[] { 0, 1 },
                Bools = new[] { true },
                Strings = new[] { "Nelibur", "TinyMapper" }
            };

            var target = _tinyMapper.Map<Target1>(source);

            XAssert.Equal(target.Ints, source.Ints);
            XAssert.Equal(target.Bools, source.Bools);
            XAssert.Equal(target.Strings, source.Strings);
        }

        [Fact]
        public void Map_Collections_Success()
        {
            _tinyMapper.Bind<Source2, Target2>();
            var source = new Source2
            {
                Bool = new List<bool> { true, false },
                Byte = new List<byte> { 0, 1 },
                Char = new List<char> { 'a', 'b' },
                Decimal = new List<decimal> { 1, 2 },
                Double = new List<double> { 2, 3 },
                Float = new List<float> { 1, 5 },
                Int = new List<int> { 0, 4, 3 },
                Long = new List<long> { 90, 23 },
                Sbyte = new List<sbyte> { 1, 1 },
                Short = new List<short> { 10, 11 },
                String = new List<string> { "f", "q" },
                Uint = new List<uint> { 9, 9 },
                Ulong = new List<ulong> { 2, 1 },
                Ushort = new List<ushort> { 5, 5 }
            };

            var target = _tinyMapper.Map<Target2>(source);

            XAssert.Equal(target.Bool, source.Bool);
            XAssert.Equal(target.Byte, source.Byte);
            XAssert.Equal(target.Char, source.Char);
            XAssert.Equal(target.Decimal, source.Decimal);
            XAssert.Equal(target.Double, source.Double);
            XAssert.Equal(target.Float, source.Float);
            XAssert.Equal(target.Int, source.Int);
            XAssert.Equal(target.Long, source.Long);
            XAssert.Equal(target.Sbyte, source.Sbyte);
            XAssert.Equal(target.Short, source.Short);
            XAssert.Equal(target.String, source.String);
            XAssert.Equal(target.Uint, source.Uint);
            XAssert.Equal(target.Ulong, source.Ulong);
            XAssert.Equal(target.Ushort, source.Ushort);
        }


        public class Source2
        {
            public IList<bool> Bool { get; set; }
            public List<byte> Byte { get; set; }
            public List<char> Char { get; set; }
            public List<decimal> Decimal { get; set; }
            public List<double> Double { get; set; }
            public List<float> Float { get; set; }
            public List<int> Int { get; set; }
            public List<long> Long { get; set; }
            public List<sbyte> Sbyte { get; set; }
            public IList<short> Short { get; set; }
            public List<string> String { get; set; }
            public List<uint> Uint { get; set; }
            public List<ulong> Ulong { get; set; }
            public List<ushort> Ushort { get; set; }
        }


        public class Target2
        {
            public IEnumerable<bool> Bool { get; set; }
            public IEnumerable<byte> Byte { get; set; }
            public IEnumerable<char> Char { get; set; }
            public IEnumerable<decimal> Decimal { get; set; }
            public IEnumerable<double> Double { get; set; }
            public IEnumerable<float> Float { get; set; }
            public IEnumerable<int> Int { get; set; }
            public IEnumerable<long> Long { get; set; }
            public IEnumerable<sbyte> Sbyte { get; set; }
            public IEnumerable<short> Short { get; set; }
            public IEnumerable<string> String { get; set; }
            public IEnumerable<uint> Uint { get; set; }
            public IEnumerable<ulong> Ulong { get; set; }
            public IEnumerable<ushort> Ushort { get; set; }
        }


        public class Source1
        {
            public bool[] Bools { get; set; }
            public int[] Ints { get; set; }
            public string[] Strings { get; set; }
        }


        public class Target1
        {
            public bool[] Bools { get; set; }
            public IEnumerable<int> Ints { get; set; }
            public IEnumerable<string> Strings { get; set; }
        }
    }
}
