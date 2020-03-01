using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Extension
{
    public static partial class UtilityExtension
    {
        #region 判断

        /// <summary>
        /// 如果<paramref name="str"/>为null或空字符串或一连串空的字符串则返回<seealso cref="bool.TrueString"/>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 如果<paramref name="str"/>为not null或空字符串或一连串空的字符串则返回<seealso cref="bool.TrueString"/>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotNullOrWhiteSpace(this string str)
        {
            return !str.IsNullOrWhiteSpace();
        }

        /// <summary>
        /// 判断<paramref name="object"/>是否为Null
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static bool IsNull(this object @object)
        {
            return @object == null;
        }

        /// <summary>
        /// 判断<paramref name="object"/>是否不为Null
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object @object)
        {
            return !@object.IsNull();
        }
        #endregion
    }
}
