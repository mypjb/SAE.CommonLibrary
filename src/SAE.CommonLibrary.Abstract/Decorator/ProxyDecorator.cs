using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Decorator
{
    /// <summary>
    /// 代理装饰器
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public sealed class ProxyDecorator<TContext> : AbstractDecorator<TContext> where TContext : DecoratorContext
    {
        /// <summary>
        /// 代理
        /// </summary>
        private readonly IDecorator<TContext> _proxy;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="decorator">装饰器</param>
        public ProxyDecorator(IDecorator<TContext> decorator)
        {
            this._proxy = decorator;
        }
        /// <inheritdoc/>
        public async override Task DecorateAsync(TContext context)
        {
            await this._proxy.DecorateAsync(context);
            await base.DecorateAsync(context);
        }

    }
}
