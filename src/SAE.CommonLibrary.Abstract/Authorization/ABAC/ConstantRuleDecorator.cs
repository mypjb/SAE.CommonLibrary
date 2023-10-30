using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.CommonLibrary.Abstract.Decorator;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 常量装饰器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConstantRuleDecorator<T> : IDecorator<RuleContext>
    {
        public ConstantRuleDecorator(T value)
        {
            this.Value = value;
        }
        protected T Value { get; }

        public async Task DecorateAsync(RuleContext context)
        {
            context.Enqueue(this.Value);
        }
    }
}