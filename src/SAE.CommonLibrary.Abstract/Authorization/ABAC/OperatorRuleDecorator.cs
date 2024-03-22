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
        /// ctor
        /// </summary>
        /// <param name="operators">逻辑操作符集合</param>
        public OperatorRuleDecorator(IEnumerable<LogicalOperator> operators)
        {
            this.Operators = operators;
        }
        /// <summary>
        /// 逻辑操作符集合
        /// </summary>
        protected IEnumerable<LogicalOperator> Operators { get; }

        ///<inheritdoc/>
        public Task DecorateAsync(RuleContext context)
        {
            var result = context.Dequeue<bool>();

            if (this.Operators.Any())
            {
                var operatorCount = this.Operators.Count();

                var i = 0;

                for (; i < operatorCount; i++)
                {
                    var @operator = this.Operators.ElementAt(i);
                    var right = context.Dequeue<bool>();
                    if (@operator == LogicalOperator.And)
                    {
                        result = result && right;
                        break;
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

                i++;
                while (i < operatorCount)
                {
                    //清空队列中剩余的值
                    context.Dequeue<bool>();
                    i++;
                }

            }

            if (result)
            {
                context.Success();
            }

            return Task.CompletedTask;
        }
    }
}