using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SAE.CommonLibrary
{
    public partial class Utils
    {
        /// <summary>
        /// 安全
        /// </summary>
        public class Security
        {
            /// <summary>
            /// MD5加密
            /// </summary>
            /// <param name="input"></param>
            /// <param name="short"></param>
            /// <returns></returns>
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
        }
    }
}
