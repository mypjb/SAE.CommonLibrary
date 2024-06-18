using SAE.Framework.Extension;
using SAE.Framework.ObjectMapper.Bindings;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.ObjectMapper
{
    public static class TinyMapperExtension
    {
        public static IObjectMapper Config<TSource, TTarget>(this IObjectMapper objectMapper, Action<ITinyMapperConfig> config)
        {
            var tinyMapper = objectMapper as TinyMapper;
            Assert.Build(tinyMapper)
                  .NotNull($"'{nameof(objectMapper)}'的实现类不是'{nameof(TinyMapper)}'");
            return tinyMapper.Config(config);
        }

        public static IObjectMapper Bind<TSource, TTarget>(this IObjectMapper objectMapper, Action<IBindingConfig<TSource, TTarget>> config)
        {
            var tinyMapper = objectMapper as TinyMapper;
            Assert.Build(tinyMapper)
                 .NotNull($"'{nameof(objectMapper)}'的实现类不是'{nameof(TinyMapper)}'");
            return tinyMapper.Bind(config);
        }
    }
}
