using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SAE.CommonLibrary
{
    public partial class Utils
    {
        /// <summary>
        /// 反射
        /// </summary>
        public class Reflection
        {
            private static ConcurrentDictionary<Type, ConcurrentDictionary<string, MemberInfo>> concurrentDictionary = new ConcurrentDictionary<Type, ConcurrentDictionary<string, MemberInfo>>();
            private static ConcurrentDictionary<string, MemberInfo> GetDictionary(Type type, BindingFlags flags = BindingFlags.Default)
            {
                return concurrentDictionary.GetOrAdd(type, key =>
                {
                    return new ConcurrentDictionary<string, MemberInfo>();
                });
            }
            
            private static string GetKey(string name, BindingFlags flags, params Type[] types)
            {
                StringBuilder sb = new StringBuilder(name);
                sb.Append("_").Append(flags.ToString());

                types?.ForEach(t=>
                {
                    sb.Append("_")
                      .Append(t.GUID);
                });

                return sb.ToString();
            }
            /// <summary>
            /// 获得<seealso cref="MemberInfo"/>
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="name"></param>
            /// <param name="flags"></param>
            /// <returns></returns>
            public static MemberInfo GetMemberInfo<T>(string name, BindingFlags flags = BindingFlags.Default)
            {
                var type = typeof(T);

                var info = GetDictionary(type).GetOrAdd(GetKey(name,flags), key =>
                {
                    return type.GetMember(name, flags)?.FirstOrDefault();
                });

                return info;
            }

            /// <summary>
            /// 获得<seealso cref="FieldInfo"/>
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="name"></param>
            /// <param name="flags"></param>
            /// <returns></returns>
            public static FieldInfo GetFieldInfo<T>(string name, BindingFlags flags = BindingFlags.Default)
            {
                var type = typeof(T);

                var info = GetDictionary(type).GetOrAdd(GetKey(name, flags), key =>
                {
                    return type.GetField(name, flags);
                });

                return info == null ? null : info as FieldInfo;
            }

            /// <summary>
            /// 获得<seealso cref="MethodInfo"/>
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="name"></param>
            /// <param name="flags"></param>
            /// <returns></returns>
            public static MethodInfo GetMethodInfo<T>(string name, BindingFlags flags = BindingFlags.InvokeMethod, params Type[] types)
            {
                var type = typeof(T);

                var info = GetDictionary(type).GetOrAdd(GetKey(name, flags, types), key =>
                {
                    return type.GetMethod(name, flags, null, types ?? Type.EmptyTypes, null);
                });

                return info == null ? null : info as MethodInfo;
            }
            /// <summary>
            /// 获得<seealso cref="PropertyInfo"/>
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="name"></param>
            /// <param name="flags"></param>
            /// <returns></returns>
            public static PropertyInfo GetPropertyInfo<T>(string name, BindingFlags flags = BindingFlags.Default)
            {
                var type = typeof(T);

                var info = GetDictionary(type).GetOrAdd(GetKey(name, flags), key =>
                {
                    return type.GetProperty(name, flags);
                });

                return info == null ? null : info as PropertyInfo;
            }

        }

    }
}
