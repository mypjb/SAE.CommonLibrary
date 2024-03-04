using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SAE.CommonLibrary.Abstract.Decorator;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 一元操作符装饰器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc/>
    public class BinaryRuleDecorator<T> : IDecorator<RuleContext> where T : IComparable, IEquatable<T>
    {
        /// <summary>
        /// 关系操作符
        /// </summary>
        /// <value></value>
        protected RelationalOperator Operator { get; }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="operator">操作符</param>
        public BinaryRuleDecorator(RelationalOperator @operator)
        {
            this.Operator = @operator;
        }
        ///<inheritdoc/>
        public async Task DecorateAsync(RuleContext context)
        {
            var left = context.Dequeue<T>();

            bool result = false;
            if (this.Operator == RelationalOperator.Not)
            {
                var boolString = left.ToString();
                result = boolString.Equals("false", StringComparison.OrdinalIgnoreCase) ||
                         boolString.Equals("0");
            }
            else
            {
                var right = context.Dequeue<T>();
                switch (this.Operator)
                {
                    case RelationalOperator.GreaterThan:
                        {
                            result = left.CompareTo(right) > 0;
                            break;
                        }
                    case RelationalOperator.LessThan:
                        {
                            result = left.CompareTo(right) < 0;
                            break;
                        }
                    case RelationalOperator.Equal:
                        {
                            result = left.CompareTo(right) == 0;
                            break;
                        }
                    case RelationalOperator.NotEqual:
                        {
                            result = left.CompareTo(right) != 0;
                            break;
                        }
                    case RelationalOperator.GreaterThanOrEqual:
                        {
                            result = left.CompareTo(right) >= 0;
                            break;
                        }
                    case RelationalOperator.LessThanOrEqual:
                        {
                            result = left.CompareTo(right) <= 0;
                            break;
                        }
                    case RelationalOperator.Include:
                        {
                            var leftString = left?.ToString();
                            var r = right?.ToString();
                            result = leftString == null || r == null ? false : leftString.IndexOf(r) != -1;
                            break;
                        }
                    case RelationalOperator.Regex:
                        {
                            var leftString = left?.ToString();
                            var r = right?.ToString() ?? string.Empty;
                            result = string.IsNullOrWhiteSpace(leftString) ? false : Regex.Match(leftString, r).Success;
                            break;
                        }
                }
            }

            context.Enqueue(result);
        }

    }
}