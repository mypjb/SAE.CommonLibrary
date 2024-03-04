using SAE.CommonLibrary.DependencyInjection;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Builder
{
    /// <summary>
    /// 指挥者代理实现
    /// </summary>
    public class Director : IDirector
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        public Director(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }
        /// <inheritdoc/>
        public virtual async Task BuildAsync<T>(T model) where T : class
        {
            if (model.IsNull()) return;
            //只有注册了具体的建筑对象才实例化它
            IEnumerable<IBuilder<T>> builders;
            if (this._serviceProvider.TryGetService(out builders))
            {
                await builders.ForEachAsync(async builder =>
                {
                    await builder.BuildAsync(model);
                });
            }
        }
    }

    ///// <summary>
    ///// 指挥者具体实现
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public class Director<T> : IDirector<T> where T : class
    //{
    //    protected IEnumerable<IBuilder<T>> _builders;

    //    public Director(IEnumerable<IBuilder<T>> builders)
    //    {
    //        this._builders = builders;
    //    }

    //    public virtual async Task Build(T model)
    //    {
    //        if (model == null) return;
    //        foreach (var builder in this._builders)
    //            await builder.Build(model);
    //    }
    //}
}
