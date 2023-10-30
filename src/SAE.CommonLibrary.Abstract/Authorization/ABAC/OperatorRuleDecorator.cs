using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.CommonLibrary.Abstract.Decorator;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 逻辑操作符装饰器
    /// </summary> 
    public class OperatorRuleDecorator : IDecorator<RuleContext>
    {
        public OperatorRuleDecorator(LogicalOperator @operator)
        {
            this.Operator = @operator;
        }
        protected LogicalOperator Operator { get; }
        public async Task DecorateAsync(RuleContext context)
        {
            var left = context.Dequeue<bool>();
            var result = false;
            
            if (this.Operator == LogicalOperator.None)
            {
                result = left;
            }
            else
            {
                var right = context.Dequeue<bool>();

                switch (this.Operator)
                {
                    case LogicalOperator.And:
                        {
                            result = left && right;
                            break;
                        }
                    case LogicalOperator.Or:
                        {
                            result = left || right;
                            break;
                        }
                }
            }

            if (result)
            {
                context.Success();
            }
        }
    }
}