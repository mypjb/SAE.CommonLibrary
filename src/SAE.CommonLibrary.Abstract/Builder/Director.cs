using SAE.CommonLibrary.DependencyInjection;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;

namespace SAE.CommonLibrary.Abstract.Builder
{
    /// <summary>
    /// 指挥者代理实现
    /// </summary>
    public class Director : IDirector
    {
        private readonly IServiceProvider _serviceProvider;

        public Director(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public virtual void Build<T>(T model) where T : class
        {
            if (model.IsNull()) return;
            //只有注册了具体的建筑对象才实例化它
            IDirector<T> director;
            if (this._serviceProvider.TryGetService(out director))
            {
                director.Build(model);
            }
        }
    }

    /// <summary>
    /// 指挥者具体实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Director<T> : IDirector<T> where T : class
    {
        protected IEnumerable<IBuilder<T>> _builders;

        public Director(IEnumerable<IBuilder<T>> builders)
        {
            this._builders = builders;
        }

        public virtual void Build(T model)
        {
            if (model == null) return;
            foreach (var builder in this._builders)
                builder.Build(model);
        }
    }
}
