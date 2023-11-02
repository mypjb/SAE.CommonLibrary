using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.CommonLibrary.Abstract.Decorator;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <inheritdoc/>
    /// <summary>
    /// 逻辑操作符装饰器
    /// </summary> 
    public class OperatorRuleDecorator : IDecorator<RuleContext>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operators"></param>
        public OperatorRuleDecorator(IEnumerable<LogicalOperator> operators)
        {
            this.Operators = operators;
        }
        /// <summary>
        /// 逻辑操作符集合
        /// </summary>
        /// <value></value>
        protected IEnumerable<LogicalOperator> Operators { get; }
        public async Task DecorateAsync(RuleContext context)
        {
            var result = context.Dequeue<bool>(); ;

            if (this.Operators.Any())
            {
                var operatorCount = this.Operators.Count();
                for (var i = 0; i < operatorCount; i++)
                {
                    var @operator = this.Operators.ElementAt(i);
                    var right = context.Dequeue<bool>();
                    if (@operator == LogicalOperator.And)
                    {
                        result = result && right;
                    }
                    else
                    {
                        result = result || right;
                        if (result)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                result = context.Dequeue<bool>();
            }

            if (result)
            {
                context.Success();
            }
        }
    }
}