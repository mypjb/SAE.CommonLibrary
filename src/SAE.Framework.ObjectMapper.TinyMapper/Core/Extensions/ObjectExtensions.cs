﻿using System;
using SAE.Framework.ObjectMapper.Core.DataStructures;

namespace SAE.Framework.ObjectMapper.Core.Extensions
{
    /// <summary>
    ///     https://github.com/Nelibur/Nelibur
    /// </summary>
    internal static class ObjectExtensions
    {
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static Option<T> ToOption<T>(this T value)
        {
            if (Helpers.IsValueType(typeof(T)) == false && ReferenceEquals(value, null))
            {
                return Option<T>.Empty;
            }
            return new Option<T>(value);
        }

        public static Option<TResult> ToType<TResult>(this object obj)
        {
            if (obj is TResult)
            {
                return new Option<TResult>((TResult)obj);
            }
            return Option<TResult>.Empty;
        }
    }
}
