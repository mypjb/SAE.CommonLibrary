using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SAE.CommonLibrary.Abstract.Decorator;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    public class DefaultRuleDecoratorBuilder : IRuleDecoratorBuilder
    {
        public virtual IDecorator<RuleContext> Build(string expression)
        {
            var (logicalOperators, nodeExpressions) = this.ParseLogicalOperator(expression);

            var propertyDecorators = new List<IDecorator<RuleContext>>();
            var relationalOperatorDecorators = new List<IDecorator<RuleContext>>();

            foreach (var nodeExpression in nodeExpressions)
            {
                var expressions = this.ParseRelationalOperator(nodeExpression);

                var tuple = this.ConvertDecorators(expressions);
                if (tuple == null)
                {
                    return null;
                }
                relationalOperatorDecorators.Add(tuple.Item1);
                propertyDecorators.AddRange(tuple.Item2);
            }

            var operatorRuleDecorator = new OperatorRuleDecorator(logicalOperators);


            ProxyDecorator<RuleContext> rootDecorator = null;

            foreach (var property in propertyDecorators)
            {
                if (rootDecorator == null)
                {
                    rootDecorator = new ProxyDecorator<RuleContext>(property);
                }
                else
                {
                    rootDecorator.Add(property);
                }
            }

            foreach (var relationalOperator in relationalOperatorDecorators)
            {
                rootDecorator.Add(relationalOperator);
            }

            rootDecorator.Add(operatorRuleDecorator);

            return rootDecorator;
        }

        /// <summary>
        /// 解析逻辑操作符
        /// </summary>
        /// <param name="expression"></param>
        protected virtual Tuple<LogicalOperator[], string[]> ParseLogicalOperator(string expression)
        {
            var count = 0;
            var start = 0;

            var matchCollection = Regex.Matches(expression, Constants.LogicalOperator);

            var logicalOperators = new LogicalOperator[matchCollection.Count];

            var equations = new string[matchCollection.Count + 1];

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

                var logicalOperator = this.ConvertLogicalOperator(match.Value);

                logicalOperators[i] = logicalOperator;

                equations[i] = expression.Substring(start, count).Trim();

                start = match.Index + match.Length;
            }

            equations[equations.Length - 1] = expression.Substring(start, expression.Length - start).Trim();

            return new Tuple<LogicalOperator[], string[]>(logicalOperators.ToArray(), equations);
        }

        /// <summary>
        /// 转换逻辑操作符
        /// </summary>
        /// <param name="str"></param>
        protected virtual LogicalOperator ConvertLogicalOperator(string str)
        {
            return str.Equals("&&") ? LogicalOperator.And : LogicalOperator.Or;
        }

        /// <summary>
        /// 转换操作符
        /// </summary>
        /// <param name="str"></param>
        protected virtual RelationalOperator ConvertRelationalOperator(string str)
        {
            RelationalOperator relationalOperator;
            switch (str)
            {
                case ">":
                    {
                        relationalOperator = RelationalOperator.GreaterThan;
                        break;
                    }
                case "<":
                    {
                        relationalOperator = RelationalOperator.LessThan;
                        break;
                    }
                case ">=":
                    {
                        relationalOperator = RelationalOperator.GreaterThanOrEqual;
                        break;
                    }
                case "<=":
                    {
                        relationalOperator = RelationalOperator.LessThanOrEqual;
                        break;
                    }
                case "==":
                    {
                        relationalOperator = RelationalOperator.Equal;
                        break;
                    }
                case "!=":
                    {
                        relationalOperator = RelationalOperator.NotEqual;
                        break;
                    }
                case "!":
                    {
                        relationalOperator = RelationalOperator.Not;
                        break;
                    }
                case "in":
                    {
                        relationalOperator = RelationalOperator.Include;
                        break;
                    }
                case "regex":
                    {
                        relationalOperator = RelationalOperator.Regex;
                        break;
                    }
                default:
                    {
                        relationalOperator = RelationalOperator.None;
                        break;
                    }
            }

            return relationalOperator;
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
        /// <summary>
        /// 构建值装饰器
        /// </summary>
        /// <param name="expressions"></param>
        protected virtual Tuple<IDecorator<RuleContext>, IDecorator<RuleContext>[]> ConvertDecorators(string[] expressions)
        {
            var relationalOperator = this.ConvertRelationalOperator(expressions[0]);


            expressions = expressions.Skip(1).ToArray();

            var constantExp = expressions.FirstOrDefault(s => !s.StartsWith(Constants.PropertyPrefix));

            IDecorator<RuleContext>[] decorates;
            IDecorator<RuleContext> relationalOperatorDecorator;

            if (relationalOperator == RelationalOperator.None)
            {
                return null;
            }
            else if (relationalOperator == RelationalOperator.Not)
            {
                decorates = this.ConvertDecoratorsCore<bool>(expressions);
                relationalOperatorDecorator = this.RelationalOperatorBuild<bool>(relationalOperator);
            }
            else
            {
                if (constantExp == null || Regex.IsMatch(constantExp, Constants.Regex.StringPattern))
                {
                    for (var i = 0; i < expressions.Length; i++)
                    {
                        var exp = expressions[i];
                        if (Regex.IsMatch(exp, Constants.Regex.StringPattern))
                        {
                            expressions[i] = exp.Substring(1, exp.Length - 2);
                        }
                    }
                    decorates = this.ConvertDecoratorsCore<string>(expressions);
                    relationalOperatorDecorator = this.RelationalOperatorBuild<string>(relationalOperator);
                }
                else if (Regex.IsMatch(constantExp, Constants.Regex.FloatPattern))
                {
                    decorates = this.ConvertDecoratorsCore<float>(expressions);
                    relationalOperatorDecorator = this.RelationalOperatorBuild<float>(relationalOperator);
                }
                else if (Regex.IsMatch(constantExp, Constants.Regex.DateTimePattern))
                {
                    decorates = this.ConvertDecoratorsCore<DateTime>(expressions);
                    relationalOperatorDecorator = this.RelationalOperatorBuild<DateTime>(relationalOperator);
                }
                else if (Regex.IsMatch(constantExp, Constants.Regex.TimeSpanPattern))
                {
                    decorates = this.ConvertDecoratorsCore<TimeSpan>(expressions);
                    relationalOperatorDecorator = this.RelationalOperatorBuild<TimeSpan>(relationalOperator);
                }
                else if (Regex.IsMatch(constantExp, Constants.Regex.BoolPattern))
                {
                    decorates = this.ConvertDecoratorsCore<bool>(expressions);
                    relationalOperatorDecorator = this.RelationalOperatorBuild<bool>(relationalOperator);
                }
                else
                {
                    decorates = Array.Empty<IDecorator<RuleContext>>();
                    relationalOperatorDecorator = null;
                }
            }

            return new Tuple<IDecorator<RuleContext>, IDecorator<RuleContext>[]>(relationalOperatorDecorator, decorates);
        }


        /// <summary>
        /// 构建值装饰器核心
        /// </summary>
        /// <param name="expressions"></param>
        /// <typeparam name="T"></typeparam>
        protected virtual IDecorator<RuleContext>[] ConvertDecoratorsCore<T>(string[] expressions)
        {
            var decorates = new IDecorator<RuleContext>[expressions.Length];
            for (var i = 0; i < expressions.Length; i++)
            {
                var exp = expressions[i];
                decorates[i] = exp.StartsWith(Constants.PropertyPrefix) ?
                                    this.PropertyBuild<T>(exp) :
                                    this.ConstantBuild(exp.ToObject<T>());
            }
            return decorates;
        }
        /// <summary>
        /// 获得属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <typeparam name="T"></typeparam>
        protected virtual IDecorator<RuleContext> PropertyBuild<T>(string propertyName)
        {
            return new PropertyRuleDecorator<T>(propertyName.Substring(1));
        }
        /// <summary>
        /// 获得常量
        /// </summary>
        /// <param name="constant"></param>
        /// <typeparam name="T"></typeparam>
        protected virtual IDecorator<RuleContext> ConstantBuild<T>(T constant)
        {
            return new ConstantRuleDecorator<T>(constant);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operator"></param>
        /// <typeparam name="T"></typeparam>
        protected virtual IDecorator<RuleContext> RelationalOperatorBuild<T>(RelationalOperator @operator) where T : IComparable, IEquatable<T>
        {
            return new BinaryRuleDecorator<T>(@operator);
        }
    }
}