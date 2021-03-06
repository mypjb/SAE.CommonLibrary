﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Decorator
{
    public abstract class AbstractDecorator<TContext> where TContext : DecoratorContext
    {
        private AbstractDecorator<TContext> decorator;
        public AbstractDecorator()
        {

        }

        public virtual void Add(IDecorator<TContext> decorator)
        {
            var abstractDecorator = decorator is AbstractDecorator<TContext> ?
                                    decorator as AbstractDecorator<TContext> :
                                    new ProxyDecorator<TContext>(decorator);
            if (this.decorator == null)
            {
                this.decorator = abstractDecorator;
            }
            else
            {
                this.decorator.Add(abstractDecorator as IDecorator<TContext>);
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
