using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Extension
{
    /// <summary>
    /// 通用函数扩展
    /// </summary>
    public static partial class  UtilityExtension
    {
        static UtilityExtension()
        {
            //extendAction = (s, t) => TinyMapper.Map(t.GetType(), s.GetType(), t, s);
        }


        #region 配置
        private static Action<object, object> extendAction;
        /// <summary>
        /// 设置对象延伸提供
        /// </summary>
        /// <param name="services"></param>
        /// <param name="extendAction"></param>
        public static void SettingExtendProvider(IServiceCollection services, Action<object, object> extendAction)
        {
            UtilityExtension.extendAction = extendAction;
        }

        #endregion

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


        public static async Task<IEnumerable<T>> ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T,Task> @delegate)
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

    }
}
