﻿using System;
using SAE.CommonLibrary.ObjectMapper;
using Xunit;

namespace SAE.CommonLibrary.ObjectMapper.Test
{
    public sealed class ForTests
    {

        [Fact]
        public void Test()
        {
            var source = new TestStaticModel();

//            TinyMapper.Bind<TestStaticModel, TestDto>();
            var dto = Map<TestStaticModel, TestDto>(source);
            // OR

            dto = source.Map<TestStaticModel, TestDto>();
        }

        private TTarget Map<TSource, TTarget>(TSource source)
        {
            if (!TinyMapper.BindingExists<TSource, TTarget>())
            {
                TinyMapper.Bind<TSource, TTarget>();
            }
            return TinyMapper.Map<TTarget>(source);
        }
    }


    public static class MyExtensions
    {
        public static TTarget Map<TSource, TTarget>(this TSource source)
        {
            if (!TinyMapper.BindingExists<TSource, TTarget>())
            {
                TinyMapper.Bind<TSource, TTarget>();
            }
            return TinyMapper.Map<TTarget>(source);
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
