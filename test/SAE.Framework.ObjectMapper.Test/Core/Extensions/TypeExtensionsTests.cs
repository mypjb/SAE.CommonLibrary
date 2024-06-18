using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SAE.Framework.ObjectMapper.Core.Extensions;
using Xunit;
using XAssert = Xunit.Assert;

namespace SAE.Framework.ObjectMapper.Test.Core.Extensions
{
    public sealed class TypeExtensionsTests
    {
        [Fact]
        public void HasDefaultCtor_MemoryStream_True()
        {
            XAssert.True((typeof(MemoryStream)).HasDefaultCtor());
        }

        [Fact]
        public void HasDefaultCtor_String_False()
        {
            XAssert.False((typeof(string)).HasDefaultCtor());
        }

        [InlineData(typeof(Dictionary<string, int>), true)]
        [InlineData(typeof(List<int>), false)]
        [Theory]
        public void IsDictionaryOf_Types_True(Type type, bool result)
        {
            XAssert.Equal(result, type.IsDictionaryOf());
        }

        [InlineData(typeof(List<int>), true)]
        [InlineData(typeof(int[]), true)]
        [InlineData(typeof(ArrayList), true)]
        [InlineData(typeof(int), false)]
        [Theory]
        public void IsIEnumerableOf_Types_True(Type type, bool result)
        {
            XAssert.Equal(result, type.IsIEnumerableOf());
        }

        [InlineData(typeof(List<int>), true)]
        [InlineData(typeof(int[]), true)]
        [InlineData(typeof(ArrayList), true)]
        [InlineData(typeof(int), false)]
        [Theory]
        public void IsIEnumerable_Types_True(Type type, bool result)
        {
            XAssert.Equal(result, type.IsIEnumerable());
        }

        [InlineData(typeof(List<int>), true)]
        [InlineData(typeof(int[]), false)]
        [Theory]
        public void IsListOf_Types_True(Type type, bool result)
        {
            XAssert.Equal(result, type.IsListOf());
        }

        [Fact]
        public void IsNullable_NotNullable_False()
        {
            XAssert.False(typeof(int).IsNullable());
        }

        [Fact]
        public void IsNullable_Nullable_True()
        {
            XAssert.True(typeof(int?).IsNullable());
        }
    }
}
