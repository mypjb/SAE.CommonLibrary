using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.Abstract.Decorator
{
    /// <summary>
    /// 装饰器接口
    /// </summary>
    /// <typeparam name="TContext">上下文</typeparam>
    public interface IDecorator<TContext> where TContext : DecoratorContext
    {
        /// <summary>
        /// 执行装饰器
        /// </summary>
        /// <param name="context">上下文</param>
        Task DecorateAsync(TContext context);
    }
}
