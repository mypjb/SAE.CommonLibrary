using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.CommonLibrary.Extension
{
    /// <summary>
    /// 通用函数扩展
    /// </summary>
    public static partial class UtilityExtension
    {

        public static int ToHttpStatusCode(this ErrorOutput errorOutput)
        {
            var code = errorOutput.StatusCode.ToString().Substring(0, 3);
            return int.Parse(code);
        }

        /// <summary>
        /// 循环<paramref name="enumerable"/>集合,并挨个执行<paramref name="action"/>函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        /// <returns></returns>
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
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        /// <returns></returns>
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

    }
}
