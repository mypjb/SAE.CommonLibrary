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
            /// <summary>
            /// 类型成员类型字典
            /// </summary>
            private static ConcurrentDictionary<Type, ConcurrentDictionary<string, MemberInfo>> concurrentDictionary = new ConcurrentDictionary<Type, ConcurrentDictionary<string, MemberInfo>>();
            /// <summary>
            /// 获得类型的成员字典对象
            /// </summary>
            /// <param name="type">类型</param>
            /// <returns>成员字典对象</returns>
            private static ConcurrentDictionary<string, MemberInfo> GetDictionary(Type type)
            {
                return concurrentDictionary.GetOrAdd(type, key =>
                {
                    return new ConcurrentDictionary<string, MemberInfo>();
                });
            }
            /// <summary>
            /// 获得类型的唯一key
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="flags">绑定参数</param>
            /// <param name="types">类型集合</param>
            /// <returns>唯一key</returns>
            private static string GetKey(string name, BindingFlags flags, params Type[] types)
            {
                StringBuilder sb = new StringBuilder(name);
                sb.Append("_").Append(flags.ToString());

                types?.ForEach(t =>
                {
                    sb.Append("_")
                      .Append(t.GUID);
                });

                return sb.ToString();
            }
            /// <summary>
            /// 获得<seealso cref="MemberInfo"/>
            /// </summary>
            /// <typeparam name="T">类型</typeparam>
            /// <param name="name">成员名称</param>
            /// <param name="flags">成员标识</param>
            /// <returns>成员信息</returns>
            public static MemberInfo GetMemberInfo<T>(string name, BindingFlags flags = BindingFlags.Default)
            {
                var type = typeof(T);

                var info = GetDictionary(type).GetOrAdd(GetKey(name, flags), key =>
                {
                    return type.GetMember(name, flags)?.FirstOrDefault();
                });

                return info;
            }

            /// <summary>
            /// 获得<seealso cref="FieldInfo"/>
            /// </summary>
            /// <typeparam name="T">类型</typeparam>
            /// <param name="name">字段名称</param>
            /// <param name="flags">字段标识</param>
            /// <returns>字段信息</returns>
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
            /// <typeparam name="T">类型</typeparam>
            /// <param name="name">方法名称</param>
            /// <param name="flags">方法标识</param>
            /// <returns>方法信息</returns>
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
            /// <typeparam name="T">类型</typeparam>
            /// <param name="name">属性名称</param>
            /// <param name="flags">属性标识</param>
            /// <returns>属性信息</returns>
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
