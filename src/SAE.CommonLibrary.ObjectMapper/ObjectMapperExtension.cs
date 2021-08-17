using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.ObjectMapper
{
    public static class ObjectMapperExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="objectMapper"></param>
        /// <returns></returns>
        public static IObjectMapper Bind<TSource, TTarget>(this IObjectMapper objectMapper)
        {
            return objectMapper.Bind(typeof(TSource),typeof(TTarget));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="objectMapper"></param>
        /// <returns></returns>
        public static bool BindingExists<TSource, TTarget>(this IObjectMapper objectMapper)
        {
            return objectMapper.BindingExists(typeof(TSource), typeof(TTarget));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="objectMapper"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget Map<TSource, TTarget>(this IObjectMapper objectMapper,TSource source)
        {
            return (TTarget)objectMapper.Map(typeof(TSource), typeof(TTarget), source);
        }
        public static TTarget Map<TSource, TTarget>(this IObjectMapper objectMapper, TSource source,TTarget target)
        {
            return (TTarget)objectMapper.Map(typeof(TSource), typeof(TTarget), source, target);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="objectMapper"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget Map<TTarget>(this IObjectMapper objectMapper, object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            return (TTarget)objectMapper.Map(source.GetType(), typeof(TTarget), source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="objectMapper"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
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
