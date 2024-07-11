using System;
using SAE.Framework.ObjectMapper.Core.DataStructures;
using SAE.Framework.ObjectMapper.Mappers;
using SAE.Framework.ObjectMapper.Mappers.Caches;
using SAE.Framework.ObjectMapper.Mappers.Classes;
using Xunit;
using XAssert = Xunit.Assert;
namespace SAE.Framework.ObjectMapper.Test.Mappers.Classes
{
    public sealed class ClassMapperTests
    {
        [Fact]
        public void Map_PrimitiveField_Success()
        {
            var builder = new ClassMapperBuilder(new MapperCache(), new MappingBuilderConfigStub());
            Mapper mapper = builder.Build(new TypePair(typeof(SourceWithFields), typeof(TargetWithFields)));

            var source = new SourceWithFields
            {
                Bool = true,
                Byte = 0,
                Char = 'a',
                Decimal = 4.0m,
                Float = 2.0f,
                Int = 9,
                Long = 2,
                Sbyte = 8,
                Short = 1,
                String = "test",
                Ulong = 3,
                Ushort = 7
            };

            var actual = (TargetWithFields)mapper.Map(source);

            XAssert.Equal(source.Bool, actual.Bool);
            XAssert.Equal(source.Byte, actual.Byte);
            XAssert.Equal(source.Char, actual.Char);
            XAssert.Equal(source.Decimal, actual.Decimal);
            XAssert.Equal(source.Float, actual.Float);
            XAssert.Equal(source.Int, actual.Int);
            XAssert.Equal(source.Long, actual.Long);
            XAssert.Equal(source.Sbyte, actual.Sbyte);
            XAssert.Equal(source.Short, actual.Short);
            XAssert.Equal(source.String, actual.String);
            XAssert.Equal(source.Ulong, actual.Ulong);
            XAssert.Equal(source.Ushort, actual.Ushort);
        }

        [Fact]
        public void Map_PrimitiveProperty_Success()
        {
            var builder = new ClassMapperBuilder(new MapperCache(), new MappingBuilderConfigStub());
            Mapper mapper = builder.Build(new TypePair(typeof(SourceWithProperties), typeof(TargetWithProperties)));

            var source = new SourceWithProperties
            {
                Bool = true,
                Byte = 0,
                Char = 'a',
                Decimal = 4.0m,
                Float = 2.0f,
                Int = 9,
                Long = 2,
                Sbyte = 8,
                Short = 1,
                String = "test",
                Ulong = 3,
                Ushort = 7,
                DateTime = new DateTime(1990, 1, 1),
                DateTimeOffset = new DateTimeOffset(new DateTime(1998, 3, 5)),
                DateTimeNullable = null,
                DateTimeNullable1 = new DateTime(2020, 2, 4)
            };

            var actual = (TargetWithProperties)mapper.Map(source);

            XAssert.Equal(source.Bool, actual.Bool);
            XAssert.Equal(source.Byte, actual.Byte);
            XAssert.Equal(source.Char, actual.Char);
            XAssert.Equal(source.Decimal, actual.Decimal);
            XAssert.Equal(source.Float, actual.Float);
            XAssert.Equal(source.Int, actual.Int);
            XAssert.Equal(source.Long, actual.Long);
            XAssert.Equal(source.Sbyte, actual.Sbyte);
            XAssert.Equal(source.Short, actual.Short);
            XAssert.Equal(source.String, actual.String);
            XAssert.Equal(source.Ulong, actual.Ulong);
            XAssert.Equal(source.Ushort, actual.Ushort);
            XAssert.Equal(source.DateTime, actual.DateTime);
            XAssert.Equal(source.DateTimeOffset, actual.DateTimeOffset);
            XAssert.Equal(source.DateTimeNullable, actual.DateTimeNullable);
            XAssert.Equal(source.DateTimeNullable1, actual.DateTimeNullable1);
        }

        public class SourceWithFields
        {
            public bool Bool;
            public byte Byte;
            public char Char;
            public decimal Decimal;
            public float Float;
            public int Int;
            public long Long;
            public sbyte Sbyte;
            public short Short;
            public string String;
            public ulong Ulong;
            public ushort Ushort;
        }

        public class TargetWithFields
        {
            public bool Bool;
            public byte Byte;
            public char Char;
            public decimal Decimal;
            public float Float;
            public int Int;
            public long Long;
            public sbyte Sbyte;
            public short Short;
            public string String;
            public ulong Ulong;
            public ushort Ushort;
        }

        public class SourceWithProperties
        {
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public char Char { get; set; }
            public DateTime DateTime { get; set; }
            public DateTime? DateTimeNullable { get; set; }
            public DateTime? DateTimeNullable1 { get; set; }
            public DateTimeOffset DateTimeOffset { get; set; }
            public decimal Decimal { get; set; }
            public float Float { get; set; }
            public int Int { get; set; }
            public long Long { get; set; }
            public sbyte Sbyte { get; set; }
            public short Short { get; set; }
            public string String { get; set; }
            public ulong Ulong { get; set; }
            public ushort Ushort { get; set; }
        }

        public class TargetWithProperties
        {
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public char Char { get; set; }
            public DateTime DateTime { get; set; }
            public DateTime? DateTimeNullable { get; set; }
            public DateTime? DateTimeNullable1 { get; set; }
            public DateTimeOffset DateTimeOffset { get; set; }
            public decimal Decimal { get; set; }
            public float Float { get; set; }
            public int Int { get; set; }
            public long Long { get; set; }
            public sbyte Sbyte { get; set; }
            public short Short { get; set; }
            public string String { get; set; }
            public ulong Ulong { get; set; }
            public ushort Ushort { get; set; }
        }
    }
}
