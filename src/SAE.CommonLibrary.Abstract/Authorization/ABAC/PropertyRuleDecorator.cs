using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.CommonLibrary.Abstract.Decorator;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 属性装饰器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc/>
    public class PropertyRuleDecorator<T> : IDecorator<RuleContext>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PropertyRuleDecorator(string propertyName)
        {
            this.PropertyName = propertyName;
        }
        /// <summary>
        /// 属性名称
        /// </summary>
        protected string PropertyName { get; }

        public async Task DecorateAsync(RuleContext context)
        {
            var val = context.Get(this.PropertyName);
            var value = string.IsNullOrWhiteSpace(val) ? default(T) : val.ToObject<T>();
            context.Enqueue(value);
        }
    }
}