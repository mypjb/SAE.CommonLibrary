using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.CommonLibrary.Abstract.Decorator;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <summary>
    /// <see cref="IDecorator{TContext}"/> 默认规则实现
    /// </summary>
    public class DefaultRuleDecorator : IDecorator<RuleContext>
    {
        ///<inheritdoc/>
        public Task DecorateAsync(RuleContext context)
        {
            return Task.CompletedTask;
        }
    }
}