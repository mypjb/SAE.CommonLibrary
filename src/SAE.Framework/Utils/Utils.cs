using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.Framework.Extension;

namespace SAE.Framework
{
    /// <summary>
    /// 工具类
    /// </summary>
    public partial class Utils
    {
        /// <summary>
        /// 生成有序的Guid
        /// </summary>
        /// <returns>guid字符串</returns>
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
        /// <returns>时间辍</returns>
        public static long Timestamp()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 返回时间戳
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns>时间辍</returns>
        public static long Timestamp(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime.ToUniversalTime()).ToUnixTimeSeconds();
        }
        /// <summary>
        /// 将集合进行切片，并逐个处理，缓和系统负载。常用与sql或API请求中的批量处理。
        /// ps:可将串行扩展为并行提高工作效率，但是会提高系统负载。
        /// </summary>
        /// <param name="array">待切片集合</param>
        /// <param name="funcTask">执行委托</param>
        /// <param name="chunkSize">切片大小</param>
        /// <typeparam name="T">切片类型</typeparam>
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
