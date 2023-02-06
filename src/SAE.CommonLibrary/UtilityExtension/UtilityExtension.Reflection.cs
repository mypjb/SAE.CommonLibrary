﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAE.CommonLibrary.Extension
{
    public static partial class UtilityExtension
	{
		/// <summary>
		/// 从程序集中检索所有继承自<paramref name="type"/>的对象
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static IEnumerable<Type> GetAssignableFrom(this Assembly assembly, Type type)
		{
			return assembly.GetTypes().Where(t => type.IsAssignableFrom(t));
		}
        /// <summary>
        /// 从程序集中检索所有继承自<typeparamref name="T"/>的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAssignableFrom<T>(this Assembly assembly) where T : class
		{
			var type = typeof(T);
			return assembly.GetAssignableFrom(type);
		}
        /// <summary>
        /// 获得类型的<paramref name="type"/>标识
        /// </summary>
        /// <remarks>
        /// 该函数使用类型的完全限定名,经过<c>md5</c>运算所得
        /// </remarks>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetIdentity(this Type type)
        {
			var identity = type.FullName.ToMd5(true).ToLower();
			return identity;
		}
	}
}
