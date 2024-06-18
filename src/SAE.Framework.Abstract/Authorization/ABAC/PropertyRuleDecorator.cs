using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.Framework.Abstract.Decorator;
using SAE.Framework.Extension;

namespace SAE.Framework.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 属性装饰器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc/>
    public class PropertyRuleDecorator<T> : IDecorator<RuleContext>
    {
        /// <summary>
        /// 属性转换器
        /// </summary>
        private readonly IPropertyConvertor<T> _propertyConvertor;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyConvertor">属性转换器</param>
        public PropertyRuleDecorator(string propertyName, IPropertyConvertor<T> propertyConvertor)
        {
            this.PropertyName = propertyName;
            this._propertyConvertor = propertyConvertor;
        }
        /// <summary>
        /// 属性名称
        /// </summary>
        protected string PropertyName { get; }
        /// <inheritdoc/>
        public Task DecorateAsync(RuleContext context)
        {
            var val = context.Get(this.PropertyName);
            var value = this._propertyConvertor.Convert(val);
            context.Enqueue(value);
            return Task.CompletedTask;
        }
    }
}