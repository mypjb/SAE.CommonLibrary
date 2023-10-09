using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary
{
    /// <summary>
    /// 工具类
    /// </summary>
    public partial class Utils
    {
        /// <summary>
        /// 生成有序的Guid
        /// </summary>
        /// <returns></returns>
        public static string GenerateId()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();
            var s = guidArray.ToBase64();
            s = s.Split('=')[0];
            s = s.Replace('+', '-').Replace('/', '_');
            return s;
        }
        /// <summary>
        /// 返回时间戳
        /// </summary>
        public static long Timestamp()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 返回时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long Timestamp(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime.ToUniversalTime()).ToUnixTimeSeconds();
        }
    }
}
