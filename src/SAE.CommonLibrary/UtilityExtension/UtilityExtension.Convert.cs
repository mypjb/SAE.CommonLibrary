using System;
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
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="input">字符串形式的枚举</param>
        /// <returns>枚举</returns>
        public static TEnum EnumTo<TEnum>(this string input) where TEnum : struct
        => EnumTo<TEnum>(o: input);

        /// <summary>
        /// 将对象<paramref name="o"/>转换成<typeparamref name="TEnum"/>
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="o">枚举</param>
        /// <returns>枚举</returns>
        private static TEnum EnumTo<TEnum>(object o) where TEnum : struct
        {
            TEnum @enum;
            Enum.TryParse(o.ToString(), out @enum);
            return @enum;
        }

        /// <summary>
        /// 将对象<paramref name="input"/>转换成<typeparamref name="TEnum"/>
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="input">int形式的枚举</param>
        /// <returns>枚举</returns>
        public static TEnum EnumTo<TEnum>(this int input) where TEnum : struct
        => EnumTo<TEnum>(o: input);

        /// <summary>
        /// 获得 <paramref name="enum"/>描述
        /// </summary>
        /// <param name="enum">枚举</param>
        /// <returns>描述</returns>
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
        /// 将<paramref name="input"/>按照<paramref name="encoding"/>编码转换成<seealso cref="byte[]"/>
        /// </summary>
        /// <param name="input">字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns>字节集合</returns>
        public static byte[] ToBytes(this string input, Encoding encoding = null)
        {
            return input.IsNullOrWhiteSpace() ? new byte[0] : (encoding ?? Constants.Encoding).GetBytes(input);
        }

        /// <summary>
        /// 将<paramref name="object"/>附加到<paramref name="target"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">主对象</param>
        /// <param name="object">附加对象</param>
        public static void Extend<T>(this T target, T @object) where T : class
        {
            if (target == null || @object == null) return;

            var type = typeof(T);
            var objectEmply = new object[0];
            foreach (var propertyInfo in type.GetProperties())
            {
                if (propertyInfo.SetMethod != null && propertyInfo.GetMethod != null)
                {
                    var value = propertyInfo.GetMethod.Invoke(@object, objectEmply);

                    if (value != null)
                    {
                        propertyInfo.SetMethod.Invoke(target, new[] { value });
                    }
                }
            }

        }
        /// <summary>
        /// 使用<paramref name="json"/> 的值填充现有对象实例。
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <param name="target">要进行填充的对象</param>
        /// <typeparam name="T"><paramref name="target"/>类型</typeparam>
        public static void JsonExtend<T>(this T target, string json) where T : class
        {
            Utils.Deserialize.PopulateObject(json, target);
        }
        #endregion
    }
}
