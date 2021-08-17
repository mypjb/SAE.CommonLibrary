using SAE.CommonLibrary.ObjectMapper;
using System;

namespace SAE.CommonLibrary.Extension
{
    public static class UtilityExtension
    {
        private static readonly Lazy<IObjectMapper> objectMapper = new Lazy<IObjectMapper>(ServiceFacade.GetService<IObjectMapper>);
        /// <summary>
        /// 映射对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static T To<T>(this object source)
        {
            if (source == null) return default(T);

            return objectMapper.Value.Map<T>(source);
        }

        /// <summary>
        /// 将<paramref name="attach"/>附加到<paramref name="source"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">对象源</param>
        /// <param name="attach">要附加的对象</param>
        public static void To<TSource, TAttach>(this TSource source, TAttach attach) where TAttach : class
        {
            Assert.Build(source)
                  .NotNull();
            Assert.Build(attach)
                  .NotNull();
            objectMapper.Value.Map(attach, source);
        }
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
