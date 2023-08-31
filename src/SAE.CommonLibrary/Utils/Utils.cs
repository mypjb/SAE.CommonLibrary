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

            // var baseDate = new DateTime(1900, 1, 1);
            // DateTime now = DateTime.Now;
            // var days = new TimeSpan(now.Ticks - baseDate.Ticks);
            // TimeSpan msecs = now.TimeOfDay;

            // byte[] daysArray = BitConverter.GetBytes(days.Days);
            // byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Array.Reverse(daysArray);
            // Array.Reverse(msecsArray);

            // Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            // Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            var s = guidArray.ToBase64();
            s = s.Split('=')[0];
            s = s.Replace('+', '-').Replace('/', '_');
            return s;
            //return new Guid(guidArray).ToString("N");
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
