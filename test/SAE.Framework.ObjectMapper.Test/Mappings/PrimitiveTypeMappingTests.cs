using System;
using SAE.Framework.ObjectMapper;
using Xunit;
using Xunit.Extensions;
using XAssert = Xunit.Assert;

namespace SAE.Framework.ObjectMapper.Test.Mappings
{
    public sealed class PrimitiveTypeMappingTests : MappingBase
    {
        public enum EnumA
        {
            A,
            B,
            C
        }

        public enum EnumB
        {
            A,
            B,
            C
        }

        [InlineData(EnumA.B, 1)]
        [InlineData(null, null)]
        [Theory]
        public void Map_EnumToNullable_Success(EnumA? sourceValue, int? expectedValue)
        {
            _tinyMapper.Bind<Source3, Target3>();

            var source = new Source3
            {
                Value = sourceValue
            };
            var result = _tinyMapper.Map<Target3>(source);
            XAssert.Equal(expectedValue, result.Value);
        }

        [InlineData(1, EnumA.B)]
        [InlineData(null, null)]
        [Theory]
        public void Map_NullableToEnum_Success(int? sourceValue, EnumA? expectedValue)
        {
            _tinyMapper.Bind<Source4, Target4>();

            var source = new Source4
            {
                Value = sourceValue
            };
            var result = _tinyMapper.Map<Target4>(source);
            XAssert.Equal(expectedValue, result.Value);
        }

        [Fact]
        public void Map_IgnoreProperties_Success()
        {
            _tinyMapper.Bind<Source2, Target2>(config =>
            {
                config.Ignore(x => x.FirstName);
                config.Ignore(x => x.LastName);
            });

            var source = new Source2
            {
                Id = 1,
                FirstName = "First",
                LastName = "LastName"
            };

            var actual = _tinyMapper.Map<Target2>(source);

            XAssert.Equal(source.Id, actual.Id);
            XAssert.Null(actual.FirstName);
            XAssert.Null(actual.LastName);
        }

        [Fact]
        public void Map_PrimitiveType_Success()
        {
            _tinyMapper.Bind<Source1, Target1>();

            var source = new Source1
            {
                Bool = true,
                Char = 'a',
                DateTime = DateTime.Now,
                IntNullable1 = 1,
                Decimal = 1m,
                DateTimeNullable1 = DateTime.Today
            };

            var actual = _tinyMapper.Map<Target1>(source);

            XAssert.Equal(source.Bool, actual.Bool);
            XAssert.Equal(source.Byte, actual.Byte);
            XAssert.Equal(source.Char, actual.Char);
            XAssert.Equal(source.Decimal, actual.Decimal);
            XAssert.Equal(source.Float, actual.Float);
            XAssert.Equal(source.Int, actual.Int);
            XAssert.Equal(source.Int1, actual.Int1);
            XAssert.Equal(source.Int2, actual.Int2);
            XAssert.Equal(source.IntNullable1.GetValueOrDefault(), actual.IntNullable1);
            XAssert.Equal(source.IntNullable2.GetValueOrDefault(), actual.IntNullable2);
            XAssert.Equal(source.Long, actual.Long);
            XAssert.Equal(source.Sbyte, actual.Sbyte);
            XAssert.Equal(source.Short, actual.Short);
            XAssert.Equal(source.String, actual.String);
            XAssert.Equal(source.Ulong, actual.Ulong);
            XAssert.Equal(source.Ushort, actual.Ushort);
            XAssert.Equal(source.DateTime, actual.DateTime);
            XAssert.Equal(source.DateTimeOffset, actual.DateTimeOffset);
            XAssert.Equal(source.DateTimeNullable1, actual.DateTimeNullable1);
            XAssert.Equal(source.DateTimeNullable2, actual.DateTimeNullable2);
        }

        public class Source1
        {
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public char Char { get; set; }
            public DateTime DateTime { get; set; }
            public DateTime? DateTimeNullable1 { get; set; }
            public DateTime? DateTimeNullable2 { get; set; }
            public DateTimeOffset DateTimeOffset { get; set; }
            public decimal Decimal { get; set; }
            public float Float { get; set; }
            public int Int { get; set; }
            public int? Int1 { get; set; }
            public int Int2 { get; set; }
            public int? IntNullable1 { get; set; }
            public int? IntNullable2 { get; set; }
            public long Long { get; set; }
            public sbyte Sbyte { get; set; }
            public short Short { get; set; }
            public string String { get; set; }
            public ulong Ulong { get; set; }
            public ushort Ushort { get; set; }
        }

        public class Source2
        {
            public string FirstName { get; set; }
            public int Id { get; set; }
            public string LastName { get; set; }
        }

        public class Source3
        {
            public EnumA? Value { get; set; }
        }

        public class Source4
        {
            public int? Value { get; set; }
        }

        public class Target1
        {
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public char Char { get; set; }
            public DateTime DateTime { get; set; }
            public DateTime? DateTimeNullable1 { get; set; }
            public DateTime? DateTimeNullable2 { get; set; }
            public DateTimeOffset DateTimeOffset { get; set; }
            public decimal Decimal { get; set; }
            public float Float { get; set; }
            public int Int { get; set; }
            public int? Int1 { get; set; }
            public int? Int2 { get; set; }
            public int IntNullable1 { get; set; }
            public int IntNullable2 { get; set; }
            public long Long { get; set; }
            public sbyte Sbyte { get; set; }
            public short Short { get; set; }
            public string String { get; set; }
            public ulong Ulong { get; set; }
            public ushort Ushort { get; set; }
        }

        public class Target2
        {
            public string FirstName { get; set; }
            public int Id { get; set; }
            public string LastName { get; set; }
        }

        public class Target3
        {
            public int? Value { get; set; }
        }

        public class Target4
        {
            public EnumA? Value { get; set; }
        }
    }
}
