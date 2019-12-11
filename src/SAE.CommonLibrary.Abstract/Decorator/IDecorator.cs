using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Decorator
{
    public interface IDecorator<TContext> where TContext : DecoratorContext
    {
        Task DecorateAsync(TContext context);
    }
}
