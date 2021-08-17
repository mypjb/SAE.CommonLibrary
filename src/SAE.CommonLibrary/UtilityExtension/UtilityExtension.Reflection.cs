using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAE.CommonLibrary.Extension
{
    public static partial class UtilityExtension
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static IEnumerable<Type> GetAssignableFrom(this Assembly assembly, Type type)
		{
			return assembly.GetTypes().Where(t => type.IsAssignableFrom(t));
		}
		/// <summary>
		/// 
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
		/// get <paramref name="type"/> identity
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string GetIdentity(this Type type)
        {
			var identity = type.FullName.ToMd5(true).ToLower();
			return identity;
		}
	}
}
