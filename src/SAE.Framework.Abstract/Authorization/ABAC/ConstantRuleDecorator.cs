using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.Framework.Abstract.Decorator;

namespace SAE.Framework.Abstract.Authorization.ABAC
{

    /// <summary>
    /// 常量装饰器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConstantRuleDecorator<T> : IDecorator<RuleContext>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="value">常量</param>
        public ConstantRuleDecorator(T value)
        {
            this.Value = value;
        }
        /// <summary>
        /// 常量值
        /// </summary>
        /// <value></value>
        protected T Value { get; }
        ///<inheritdoc/>
        public async Task DecorateAsync(RuleContext context)
        {
            context.Enqueue(this.Value);
        }
    }
}