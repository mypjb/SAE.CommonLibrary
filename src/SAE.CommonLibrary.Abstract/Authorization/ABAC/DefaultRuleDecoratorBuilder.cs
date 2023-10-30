using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SAE.CommonLibrary.Abstract.Decorator;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    public class DefaultRuleDecoratorBuilder : IRuleDecoratorBuilder
    {
        public virtual IDecorator<RuleContext> Build(string expression)
        {
            var logicalOperators = this.ParseLogicalOperator(expression);

            foreach (var op in logicalOperators)
            {
                var relationalOperators = this.ParseRelationalOperator(op);
            }
            
            return null;
        }
        /// <summary>
        /// 解析逻辑操作符
        /// </summary>
        /// <param name="expression"></param>
        protected virtual string[] ParseLogicalOperator(string expression)
        {
            var matchCollection = Regex.Matches(expression, Constants.LogicalOperator);

            var equations = new string[matchCollection.Count + 1];
            var count = 0;
            var start = 0;
            for (var i = 0; i < matchCollection.Count; i++)
            {
                var match = matchCollection[i];

                if (i == 0)
                {
                    count = match.Index;
                }
                else
                {
                    count = match.Index - start;
                }
                equations[i] = expression.Substring(start, count).Trim();
                start = match.Index + match.Length;
            }

            equations[equations.Length - 1] = expression.Substring(start, expression.Length - start).Trim();

            return equations;
        }
        /// <summary>
        /// 解析关系操作符
        /// </summary>
        /// <param name="expression"></param>
        protected virtual string[] ParseRelationalOperator(string expression)
        {
            var operatorMatch = Regex.Match(expression, Constants.RelationalOperator);

            if (operatorMatch.Success)
            {
                if (operatorMatch.Index == 0)
                {
                    return new string[] { operatorMatch.Value, expression.Substring(operatorMatch.Index + operatorMatch.Length).Trim() };
                }
                else
                {
                    var left = expression.Substring(0, operatorMatch.Index).Trim();
                    var right = expression.Substring(operatorMatch.Index + operatorMatch.Length).Trim();
                    return new string[] { operatorMatch.Value, left, right };
                }
            }

            return Array.Empty<string>();
        }
    }
}