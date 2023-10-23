using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
        /// <summary>
        /// 将集合进行切片，并逐个处理，缓和系统负载。常用与sql或API请求中的批量处理。
        /// ps:可将串行扩展为并行提高工作效率，但是会提高系统负载。
        /// </summary>
        /// <param name="array"></param>
        /// <param name="funcTask"></param>
        /// <param name="chunkSize"></param>
        /// <typeparam name="T"></typeparam>
        public static async Task SliceAsync<T>(IEnumerable<T> array, Func<IEnumerable<T>, Task> funcTask, int chunkSize = 1000)
        {
            if (array == null) return;
            var index = 0;
            var count = array.Count();
            while (count > index)
            {
                if (count - index < chunkSize)
                {
                    chunkSize = count - index;
                }
                await funcTask(array.Skip(index).Take(chunkSize).ToArray());
                index += chunkSize;
            };
        }
    }
}
