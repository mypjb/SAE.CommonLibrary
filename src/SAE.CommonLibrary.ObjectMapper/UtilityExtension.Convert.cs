using SAE.CommonLibrary.ObjectMapper;
using System;

namespace SAE.CommonLibrary.Extension
{
    /// <summary>
    /// 基于<see cref="IObjectMapper"/>对象映射扩展
    /// </summary>
    public static class UtilityExtension
    {
        private static readonly Lazy<IObjectMapper> objectMapper = new Lazy<IObjectMapper>(ServiceFacade.GetService<IObjectMapper>);
        /// <summary>
        /// 映射对象
        /// </summary>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源</param>
        /// <returns>目标对象</returns>
        public static TTarget To<TTarget>(this object source)
        {
            if (source == null) return default(TTarget);

            return objectMapper.Value.Map<TTarget>(source);
        }

        /// <summary>
        /// 将<paramref name="attach"/>附加到<paramref name="source"/>
        /// </summary>
        /// <typeparam name="TSource">要附加的类型</typeparam>
        /// <typeparam name="TAttach">附加类型</typeparam>
        /// <param name="source">要附加到的对象</param>
        /// <param name="attach">附加对象</param>
        public static void To<TSource, TAttach>(this TSource source, TAttach attach) where TAttach : class
        {
            Assert.Build(source)
                  .NotNull();
            Assert.Build(attach)
                  .NotNull();
            objectMapper.Value.Map(attach, source);
        }
        /// <summary>
        /// 将<paramref name="attach"/>附加到<paramref name="source"/>
        /// </summary>
        /// <param name="source">要附加到的对象</param>
        /// <param name="sourceType">要附加的类型</param>
        /// <param name="attachType">附加类型</param>
        /// <param name="attach">附加对象</param>
        public static void To(this object source,Type sourceType,Type attachType,object attach)
        {
            Assert.Build(source)
                  .NotNull();
            Assert.Build(attach)
                  .NotNull();
            objectMapper.Value.Map(attachType, sourceType, attach, source);
        }
    }
}
