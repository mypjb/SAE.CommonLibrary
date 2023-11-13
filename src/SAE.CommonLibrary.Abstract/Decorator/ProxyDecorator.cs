using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Decorator
{
    public sealed class ProxyDecorator<TContext> : AbstractDecorator<TContext> where TContext : DecoratorContext
    {
        private readonly IDecorator<TContext> _proxy;

        public ProxyDecorator(IDecorator<TContext> decorator)
        {
            this._proxy = decorator;
        }

        public async override Task DecorateAsync(TContext context)
        {
            await this._proxy.DecorateAsync(context);
            await base.DecorateAsync(context);
        }

    }
}
