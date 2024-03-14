using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.ObjectMapper
{
    /// <summary>
    /// <see cref="IObjectMapper"/>映射扩展
    /// </summary>
    public static class ObjectMapperExtension
    {
        /// <summary>
        /// 绑定
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="objectMapper">映射接口</param>
        /// <returns><paramref name="objectMapper"/></returns>
        public static IObjectMapper Bind<TSource, TTarget>(this IObjectMapper objectMapper)
        {
            return objectMapper.Bind(typeof(TSource),typeof(TTarget));
        }
        /// <summary>
        /// 绑定是否存在
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="objectMapper">映射接口</param>
        /// <returns>true:存在</returns>
        public static bool BindingExists<TSource, TTarget>(this IObjectMapper objectMapper)
        {
            return objectMapper.BindingExists(typeof(TSource), typeof(TTarget));
        }
        /// <summary>
        /// 映射对象
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="objectMapper">映射接口</param>
        /// <param name="source">源</param>
        /// <returns>映射后的对象</returns>
        public static TTarget Map<TSource, TTarget>(this IObjectMapper objectMapper,TSource source)
        {
            return (TTarget)objectMapper.Map(typeof(TSource), typeof(TTarget), source);
        }
        /// <summary>
        /// 映射对象
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="objectMapper">映射接口</param>
        /// <param name="source">源</param>
        /// <param name="target">目标，如果目标已存在，则采用附加的形式</param>
        /// <returns>映射后的对象</returns>
        public static TTarget Map<TSource, TTarget>(this IObjectMapper objectMapper, TSource source,TTarget target)
        {
            return (TTarget)objectMapper.Map(typeof(TSource), typeof(TTarget), source, target);
        }
        /// <summary>
        /// 映射对象
        /// </summary>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="objectMapper">映射接口</param>
        /// <param name="source">源</param>
        /// <returns>映射后的对象</returns>
        public static TTarget Map<TTarget>(this IObjectMapper objectMapper, object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            return (TTarget)objectMapper.Map(source.GetType(), typeof(TTarget), source);
        }

        /// <summary>
        /// 映射对象
        /// </summary>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="objectMapper">映射接口</param>
        /// <param name="source">源</param>
        /// <param name="target">目标，如果目标已存在，则采用附加的形式</param>
        /// <returns>映射后的对象</returns>
        public static TTarget Map<TTarget>(this IObjectMapper objectMapper, object source,TTarget target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            return (TTarget)objectMapper.Map(source.GetType(), typeof(TTarget), source, target);
        }
    }
}
