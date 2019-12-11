using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        => EnumTo<TEnum>(input);


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
        => EnumTo<TEnum>(input);

        /// <summary>
        /// 获得<paramref name="enum"/>Display“Name”
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string Display(this Enum @enum)
        {
            var name = @enum.ToString();
            var field = @enum.GetType().GetField(name);
            if (field == null) return name;
            var attribute = field.GetCustomAttributes(false).OfType<DisplayAttribute>().FirstOrDefault();
            return attribute == null ? field.Name : attribute.GetName();
        }

        /// <summary>
        /// 映射对象
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static TModel To<TModel>(this object o)
        {
            if (o == null) return default(TModel);
            throw new NotImplementedException();
            //return TinyMapper.Map<TModel>(o);
        }

        /// <summary>
        /// 将<paramref name="attach"/>附加到<paramref name="source"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">对象源</param>
        /// <param name="attach">要附加的对象</param>
        public static void Extend<T>(this T source, object attach) where T : class
        {
            Assert.Build(source)
                  .NotNull();
            Assert.Build(attach)
                  .NotNull();
            extendAction.Invoke(source, attach);
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
        #endregion
    }
}
