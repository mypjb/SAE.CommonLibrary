using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.Framework.Extension
{
    /// <summary>
    /// 通用函数扩展
    /// </summary>
    public static partial class UtilityExtension
    {
        /// <summary>
        /// 转换成http状态码
        /// </summary>
        /// <param name="errorOutput">错误输出</param>
        /// <returns>返回错误码</returns>

        public static int ToHttpStatusCode(this ErrorOutput errorOutput)
        {
            var code = errorOutput.StatusCode.ToString().Substring(0, 3);
            return int.Parse(code);
        }

        /// <summary>
        /// 循环<paramref name="enumerable"/>集合,并挨个执行<paramref name="action"/>函数
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <param name="enumerable">集合</param>
        /// <param name="action">循环函数</param>
        /// <returns><paramref name="enumerable"/></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable != null)
            {
                foreach (var itm in enumerable)
                {
                    action(itm);
                }
            }
            return enumerable;
        }

        /// <summary>
        /// 循环<paramref name="enumerable"/>集合,并挨个执行<paramref name="action"/>函数
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <param name="enumerable">集合类型</param>
        /// <param name="action">循环函数</param>
        /// <returns><paramref name="enumerable"/></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            if (enumerable != null)
            {
                var i = 0;
                foreach (var itm in enumerable)
                {
                    action(itm, i++);
                }
            }
            return enumerable;
        }

        /// <summary>
        /// 循环<paramref name="enumerable"/>集合,并挨个执行<paramref name="delegate"/>函数
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <param name="enumerable">集合类型</param>
        /// <param name="delegate">循环函数</param>
        /// <returns><paramref name="enumerable"/></returns>
        public static async Task<IEnumerable<T>> ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> @delegate)
        {
            if (enumerable != null)
            {
                foreach (var itm in enumerable)
                {
                    await @delegate(itm);
                }
            }
            return enumerable;
        }
        /// <summary>
        /// 循环<paramref name="enumerable"/>集合,并挨个执行<paramref name="delegate"/>函数
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <param name="enumerable">集合类型</param>
        /// <param name="delegate">循环函数</param>
        /// <returns><paramref name="enumerable"/></returns>
        public static async Task<IEnumerable<T>> ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, int, Task> @delegate)
        {
            if (enumerable != null)
            {
                var i = 0;
                foreach (var itm in enumerable)
                {
                    await @delegate(itm, i++);
                }
            }
            return enumerable;
        }
        /// <summary>
        /// 获得时间戳
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns>时间辍</returns> 
        public static long ToTimestamp(this DateTime dateTime)
        {
            return Utils.Timestamp(dateTime);
        }

    }
}
