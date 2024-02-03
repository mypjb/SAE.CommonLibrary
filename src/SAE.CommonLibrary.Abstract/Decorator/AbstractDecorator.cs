using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Decorator
{
    /// <summary>
    /// 接口<seealso cref="IDecorator{TContext}"/>抽象实现
    /// </summary>
    /// <typeparam name="TContext">上下文</typeparam>
    /// <inheritdoc/>
    public abstract class AbstractDecorator<TContext> : IDecorator<TContext> where TContext : DecoratorContext
    {
        private AbstractDecorator<TContext> decorator;
        /// <summary>
        /// ctor
        /// </summary>
        public AbstractDecorator()
        {

        }
        /// <summary>
        /// 添加装饰器,这会把装饰其累加到下一级
        /// </summary>
        /// <param name="decorator"></param>
        public virtual void Add(IDecorator<TContext> decorator)
        {
            var abstractDecorator = decorator is AbstractDecorator<TContext> ?
                                    (AbstractDecorator<TContext>)decorator :
                                    new ProxyDecorator<TContext>(decorator);
            if (this.decorator == null)
            {
                this.decorator = abstractDecorator;
            }
            else
            {
                this.decorator.Add(abstractDecorator);
            }
        }

        public virtual async Task DecorateAsync(TContext context)
        {
            if (this.decorator != null && !context.Complete)
            {
                await this.decorator.DecorateAsync(context);
            }
        }
    }
}
