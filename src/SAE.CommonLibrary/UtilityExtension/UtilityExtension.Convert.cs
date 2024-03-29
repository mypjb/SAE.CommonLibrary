﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Extension
{
    public static partial class UtilityExtension
    {
        #region 转换
        /// <summary>
        /// 将对象<paramref name="input"/>转换成<typeparamref name="TEnum"/>
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TEnum EnumTo<TEnum>(this string input) where TEnum : struct
        => EnumTo<TEnum>(o: input);


        private static TEnum EnumTo<TEnum>(object o) where TEnum : struct
        {
            TEnum @enum;
            Enum.TryParse(o.ToString(), out @enum);
            return @enum;
        }

        /// <summary>
        /// 将对象<paramref name="input"/>转换成<typeparamref name="TEnum"/>
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TEnum EnumTo<TEnum>(this int input) where TEnum : struct
        => EnumTo<TEnum>(o: input);

        /// <summary>
        /// get <paramref name="enum"/> detail
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string GetDetail(this Enum @enum)
        {
            var str = @enum.ToString();

            var sb = new StringBuilder();

            for (int i = 0; i < str.Length; i++)
            {
                if (i > 0 && str[i] >= 'A' && str[i] < 'a')
                {
                    sb.Append(' ');
                }
                sb.Append(str[i]);
            }

            return sb.ToString();
            //var name = @enum.ToString();
            //var field = @enum.GetType().GetField(name);
            //if (field == null) return name;
            //var attribute = field.GetCustomAttributes(false).OfType<DisplayAttribute>().FirstOrDefault();
            //return attribute == null ? field.Name : attribute.GetName();
        }


        /// <summary>
        /// 将<paramref name="input"/>按照<paramref name="encoding"/>编码转换成<seealso cref="IEnumerable{byte}"/>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static IEnumerable<byte> ToBytes(this string input, Encoding encoding = null)
        {
            return input.IsNullOrWhiteSpace() ? new byte[0] : (encoding ?? Constant.Encoding).GetBytes(input);
        }

        /// <summary>
        /// We append <paramref name="object"/> to <paramref name="target"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="object"></param>
        public static void Extend<T>(this T target, T @object) where T : class
        {
            if (target == null || @object == null) return;

            var type = typeof(T);
            var objectEmply = new object[0];
            foreach (var propertyInfo in type.GetProperties())
            {
                if (propertyInfo.SetMethod != null && propertyInfo.GetMethod != null)
                {
                    var value= propertyInfo.GetMethod.Invoke(@object, objectEmply);

                    if (value != null)
                    {
                        propertyInfo.SetMethod.Invoke(target, new[] { value });
                    }
                }
            }

        }
        #endregion
    }
}
