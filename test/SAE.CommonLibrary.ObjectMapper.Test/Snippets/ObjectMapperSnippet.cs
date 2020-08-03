using System;
using SAE.CommonLibrary.ObjectMapper;
using SAE.CommonLibrary.ObjectMapper.Bindings;

namespace SAE.CommonLibrary.ObjectMapper.Test.Snippets
{
    public sealed class ObjectMapperSnippet : MappingBase,IObjectMapper
    {
        public void Bind<TSource, TTarget>()
        {
            _tinyMapper.Bind<TSource, TTarget>();
        }

        public void Bind<TSource, TTarget>(Action<IBindingConfig<TSource, TTarget>> config)
        {
            _tinyMapper.Bind(config);
        }

        public TTarget Map<TSource, TTarget>(TSource source, TTarget target = default(TTarget))
        {
            return _tinyMapper.Map(source, target);
        }

        public TTarget Map<TTarget>(object source)
        {
            return _tinyMapper.Map<TTarget>(source);
        }
    }

    public interface IObjectMapper
    {
        void Bind<TSource, TTarget>();
        void Bind<TSource, TTarget>(Action<IBindingConfig<TSource, TTarget>> config);
        TTarget Map<TSource, TTarget>(TSource source, TTarget target = default(TTarget));
        TTarget Map<TTarget>(object source);
    }
}
