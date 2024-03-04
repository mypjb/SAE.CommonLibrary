using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SAE.CommonLibrary.Extension
{
    public static partial class UtilityExtension
    {
        #region 加密
        /// <summary>
        /// 将<paramref name="str"/>转换成MD5
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <param name="short">true16位MD5,否则32位MD5</param>
        /// <returns>返回加密后的字符</returns>
        public static string ToMd5(this string str, bool @short = false)
        {
            return Utils.Security.MD5(str, @short);
        }
        /// <summary>
        /// 将<paramref name="stream"/>转换成MD5
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="short">true16位MD5,否则32位MD5</param>
        /// <returns>返回加密后的字符</returns>
        public static string ToMd5(this Stream stream, bool @short = false)
        {
            return Utils.Security.MD5(stream, @short);
        }
        /// <summary>
        /// 将<paramref name="bytes"/>转换成MD5
        /// </summary>
        /// <param name="bytes">字节集合</param>
        /// <param name="short">true16位MD5,否则32位MD5</param>
        /// <returns>返回加密后的字符</returns>
        public static string ToMd5(this IEnumerable<byte> bytes, bool @short = false)
        {
            return Utils.Security.MD5(bytes, @short);
        }
        /// <summary>
        /// 将<paramref name="bytes"/>转换成base64字符串
        /// </summary>
        /// <param name="bytes">字节集合</param>
        /// <returns>返回base64字符串</returns>
        public static string ToBase64(this IEnumerable<byte> bytes)
        {
            return Utils.Security.Base64(bytes);
        }
        #endregion
    }
}
