using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.CommonLibrary.Abstract.Decorator;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    public class DefaultRuleDecorator : IDecorator<RuleContext>
    {
        public Task DecorateAsync(RuleContext context)
        {
            return Task.CompletedTask;
        }
    }
}