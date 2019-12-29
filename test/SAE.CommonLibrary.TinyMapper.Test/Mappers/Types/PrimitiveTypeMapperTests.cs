﻿using System;
using SAE.CommonLibrary.ObjectMapper.Core.DataStructures;
using SAE.CommonLibrary.ObjectMapper.Mappers;
using SAE.CommonLibrary.ObjectMapper.Mappers.Types.Convertible;
using Xunit;

namespace SAE.CommonLibrary.ObjectMapper.Test.Mappers.Types
{
    public sealed class PrimitiveTypeMapperTests
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

        [InlineData(EnumA.A, EnumB.A)]
        [InlineData(EnumA.A, 0)]
        [Theory]
        public void Map_Enum_Success(object source, object expected)
        {
            var builder = new ConvertibleTypeMapperBuilder(new MappingBuilderConfigStub());
            Mapper mapper = builder.Build(new TypePair(typeof(EnumA), typeof(EnumB)));

            var actual = (EnumB)mapper.Map(source);
            Assert.Equal((EnumB)expected, actual);
        }

        [InlineData(typeof(bool), typeof(bool), true, true)]
        [InlineData(typeof(byte), typeof(byte), 0, 0)]
        [InlineData(typeof(int), typeof(int), 1, 1)]
        [InlineData(typeof(int), typeof(int?), 5, 5)]
        [InlineData(typeof(int?), typeof(int?), 5, 5)]
        [InlineData(typeof(int?), typeof(int), 5, 5)]
        [InlineData(typeof(string), typeof(int), "1", 1)]
        [InlineData(typeof(int), typeof(string), 1, "1")]
        [InlineData(typeof(decimal), typeof(decimal), 5, 5)]
        [InlineData(typeof(float), typeof(float), 6, 6)]
        [InlineData(typeof(long), typeof(long), 7, 7)]
        [InlineData(typeof(ulong), typeof(ulong), 2, 2)]
        [InlineData(typeof(sbyte), typeof(sbyte), 8, 8)]
        [InlineData(typeof(short), typeof(short), 10, 10)]
        [InlineData(typeof(ushort), typeof(ushort), 10, 10)]
        [InlineData(typeof(char), typeof(char), 'a', 'a')]
        [InlineData(typeof(string), typeof(string), "abc", "abc")]
        [Theory]
        public void Map_PrimitiveTypes_Success(Type sourceType, Type targetType, object source, object expected)
        {
            var builder = new ConvertibleTypeMapperBuilder(new MappingBuilderConfigStub());
            Mapper mapper = builder.Build(new TypePair(sourceType, targetType));
            object actual = mapper.Map(source);
            Assert.Equal(expected, actual);
        }
    }
}
