using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SAE.CommonLibrary.Abstract.Decorator;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <inheritdoc/>
    /// <summary>
    /// <see cref="IRuleDecoratorBuilder"/>默认实现
    /// </summary>
    public class DefaultRuleDecoratorBuilder : IRuleDecoratorBuilder
    {
        private readonly ILogging _logging;

        public DefaultRuleDecoratorBuilder(ILogging<DefaultRuleDecoratorBuilder> logging)
        {
            this._logging = logging;
        }
        public virtual IDecorator<RuleContext> Build(string expression)
        {
            this._logging.Info($"开始解析表达式：{expression}");

            var (logicalOperators, nodeExpressions) = this.ParseLogicalOperator(expression);

            var propertyDecorators = new List<IDecorator<RuleContext>>();
            var relationalOperatorDecorators = new List<IDecorator<RuleContext>>();

            foreach (var nodeExpression in nodeExpressions)
            {
                this._logging.Info($"解析节点:{nodeExpression}");

                var expressions = this.ParseRelationalOperator(nodeExpression);

                if (expressions.Length == 0)
                {
                    this._logging.Error($"表达式'{expression}'所属子表达式'{nodeExpression}'解析关系符时失败，该条规则将会被遗弃。");
                    return null;
                }
                var tuple = this.ConvertDecorators(expressions);
                if (tuple == null)
                {
                    this._logging.Error($"表达式'{expression}'所属子表达式'{nodeExpression}'转换时'{nameof(ConvertDecorators)}'失败，该条规则将会被遗弃。");
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
                if (rootDecorator == null)
                {
                    rootDecorator = new ProxyDecorator<RuleContext>(relationalOperator);
                }
                else
                {
                    rootDecorator.Add(relationalOperator);
                }
            }

            rootDecorator.Add(operatorRuleDecorator);

            this._logging.Info($"表达式'{expression}',存在{propertyDecorators.Count}个属性、{relationalOperatorDecorators.Count}个关系符、{logicalOperators.Length}个逻辑符");

            return rootDecorator;
        }

        /// <summary>
        /// 解析逻辑操作符
        /// </summary>
        /// <param name="expression"></param>
        protected virtual Tuple<LogicalOperator[], string[]> ParseLogicalOperator(string expression)
        {
            this._logging.Info($"开始解析逻辑操作符:{expression}");
            var count = 0;
            var start = 0;

            var matchCollection = Regex.Matches(expression, Constants.Regex.LogicalOperatorPattern);

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
            this._logging.Info($"'{expression}'解析完成，存在逻辑符:{logicalOperators.Length}个");
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
            this._logging.Debug($"关系符转换 {str} -> {relationalOperator}");
            return relationalOperator;
        }

        /// <summary>
        /// 解析关系操作符
        /// </summary>
        /// <param name="expression"></param>
        protected virtual string[] ParseRelationalOperator(string expression)
        {
            string[] exps;

            var operatorMatch = Regex.Match(expression, Constants.Regex.RelationalOperatorPattern);

            if (operatorMatch.Success)
            {
                exps = new string[2];
                exps[0] = expression.Substring(0, operatorMatch.Length).Trim();
                exps[exps.Length - 1] = expression.Substring(operatorMatch.Length).Trim();
            }
            else
            {
                var valuePattern = Regex.Match(expression, Constants.Regex.ValuePattern);
                exps = new string[3];
                exps[1] = valuePattern.Value;
                expression = expression.Substring(valuePattern.Length).Trim();
                operatorMatch = Regex.Match(expression, Constants.Regex.RelationalOperatorPattern);

                if (operatorMatch.Success)
                {
                    exps[0] = expression.Substring(0, operatorMatch.Length).Trim();
                    exps[exps.Length - 1] = expression.Substring(operatorMatch.Length).Trim();
                }
                else
                {
                    exps = new string[0];
                }
            }
            this._logging.Debug($"关系符解析：{expression} -> {(exps.Length == 0 ? string.Empty : exps.ToJsonString())}");
            return exps;
        }
        /// <summary>
        /// 构建值装饰器
        /// </summary>
        /// <param name="expressions"></param>
        protected virtual Tuple<IDecorator<RuleContext>, IDecorator<RuleContext>[]> ConvertDecorators(string[] expressions)
        {
            var relationalOperator = this.ConvertRelationalOperator(expressions[0]);

            expressions = expressions.Skip(1).ToArray();

            var constantExp = expressions.FirstOrDefault(s => !Regex.IsMatch(s, Constants.Regex.PropertyPattern));

            IDecorator<RuleContext>[] decorates;
            IDecorator<RuleContext> relationalOperatorDecorator;

            var message = $"属性符号：{expressions.ToJsonString()}";

            if (relationalOperator == RelationalOperator.None)
            {
                this._logging.Error($"不存在关系符终止解析：{expressions[0]}");
                return null;
            }
            else if (relationalOperator == RelationalOperator.Not)
            {
                decorates = this.ConvertDecoratorsCore<bool>(expressions);
                relationalOperatorDecorator = this.RelationalOperatorBuild<bool>(relationalOperator);
                this._logging.Info($"关系符为'!',对应属性强制转换为bool类型。{message}");
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
                            expressions[i] = exp.Substring(1, exp.Length - 2)
                                                .Replace("\\&", "&")
                                                .Replace("\\|", "|");
                        }
                    }
                    decorates = this.ConvertDecoratorsCore<string>(expressions);
                    relationalOperatorDecorator = this.RelationalOperatorBuild<string>(relationalOperator);
                    this._logging.Info($"将对象转换为'string'。{message}");
                }
                else if (Regex.IsMatch(constantExp, Constants.Regex.FloatPattern))
                {
                    decorates = this.ConvertDecoratorsCore<float>(expressions);
                    relationalOperatorDecorator = this.RelationalOperatorBuild<float>(relationalOperator);
                    this._logging.Info($"将对象转换为'float'。{message}");
                }
                else if (Regex.IsMatch(constantExp, Constants.Regex.DateTimePattern))
                {
                    decorates = this.ConvertDecoratorsCore<DateTime>(expressions);
                    relationalOperatorDecorator = this.RelationalOperatorBuild<DateTime>(relationalOperator);
                    this._logging.Info($"将对象转换为'DateTime'。{message}");
                }
                else if (Regex.IsMatch(constantExp, Constants.Regex.TimeSpanPattern))
                {
                    decorates = this.ConvertDecoratorsCore<TimeSpan>(expressions);
                    relationalOperatorDecorator = this.RelationalOperatorBuild<TimeSpan>(relationalOperator);
                    this._logging.Info($"将对象转换为'TimeSpan'。{message}");
                }
                else if (Regex.IsMatch(constantExp, Constants.Regex.BoolPattern))
                {
                    decorates = this.ConvertDecoratorsCore<bool>(expressions);
                    relationalOperatorDecorator = this.RelationalOperatorBuild<bool>(relationalOperator);
                    this._logging.Info($"将对象转换为'bool'。{message}");
                }
                else
                {
                    this._logging.Error($"解析失败。{message}");
                    return null;
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
                var match = Regex.Match(exp, Constants.Regex.PropertyPattern);
                decorates[i] = match.Success ?
                                this.PropertyBuild<T>(match.Groups[1].Value) :
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
            return new PropertyRuleDecorator<T>(propertyName);
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