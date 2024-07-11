using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SAE.Framework.Extension;

namespace SAE.Framework
{
    public partial class Utils
    {
        /// <summary>
        /// 安全
        /// </summary>
        public class Security
        {
            /// <summary>
            /// <c>md5</c>加密
            /// </summary>
            /// <param name="input">加密字符串</param>
            /// <param name="short">true:16位加密，false:32位</param>
            /// <returns>加密后的字符串(大写)</returns>
            public static string MD5(string input, bool @short = false)
            {
                if (input.IsNullOrWhiteSpace()) return input;
                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    var result = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                    string strResult;
                    if (@short)
                    {
                        strResult = BitConverter.ToString(result, 4, 8);
                    }
                    else
                    {
                        strResult = BitConverter.ToString(result);
                    }
                    return strResult.Replace("-", "");
                }
            }
            /// <summary>
            /// <c>md5</c>加密
            /// </summary>
            /// <param name="bytes">加密字节</param>
            /// <param name="short">true:16位加密，false:32位</param>
            /// <returns>加密后的字符串(大写)</returns>
            public static string MD5(IEnumerable<byte> bytes, bool @short = false)
            {
                if (bytes == null || !bytes.Any()) return string.Empty;

                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    var result = md5.ComputeHash(bytes.ToArray());
                    string strResult;
                    if (@short)
                    {
                        strResult = BitConverter.ToString(result, 4, 8);
                    }
                    else
                    {
                        strResult = BitConverter.ToString(result);
                    }
                    return strResult.Replace("-", "");
                }
            }
            /// <summary>
            /// <c>md5</c>加密
            /// </summary>
            /// <param name="stream">加密流</param>
            /// <param name="short">true:16位加密，false:32位</param>
            /// <returns>加密后的字符串(大写)</returns>
            public static string MD5(Stream stream, bool @short = false)
            {
                if (stream == null) return string.Empty;

                var position = stream.Position;

                var md5Str = string.Empty;

                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    var result = md5.ComputeHash(stream);
                    string strResult;
                    if (@short)
                    {
                        strResult = BitConverter.ToString(result, 4, 8);
                    }
                    else
                    {
                        strResult = BitConverter.ToString(result);
                    }
                    md5Str = strResult.Replace("-", "");
                }

                stream.Position = position;

                return md5Str;
            }
            /// <summary>
            /// 转换成<c>base64</c>
            /// </summary>
            /// <param name="bytes">加密字节</param>
            /// <returns>base64字符串</returns>
            public static string Base64(IEnumerable<byte> bytes)
            {
                return Convert.ToBase64String(bytes.ToArray());
            }
        }
    }
}
