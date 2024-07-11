using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.Framework.Abstract.Decorator;

namespace SAE.Framework.Abstract.Authorization.ABAC
{
    /// <summary>
    /// <see cref="IDecorator{RuleContext}"/>构造器
    /// </summary>
    public interface IRuleDecoratorBuilder
    {
        /// <summary>
        /// 使用<paramref name="expression"/>构造<see cref="IDecorator{RuleContext}"/>对象
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>装饰器</returns>
        IDecorator<RuleContext> Build(string expression);
    }
}