using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SAE.CommonLibrary.Abstract.Responsibility;
using SAE.CommonLibrary.AspNetCore.Authorization.ABAC;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Test;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.AspNetCore.Test
{
    public class ABACAuthorizeTest : BaseTest
    {
        private ABACTextResolver _textResolver;

        public ABACAuthorizeTest(ITestOutputHelper output) : base(output)
        {
            this._textResolver = new ABACTextResolver();
        }

        [Theory]
        [InlineData("18 >= $age || 'student' != $role || 'pjb' == $name")]
        public async Task AuthAsync(string arg)
        {
            _textResolver.Build(arg);
        }
    }
    public class ABACTextResolver
    {
        /// <summary>
        /// 逻辑操作符
        /// </summary>
        public const string LogicalOperator = @"(\|\|)|(&&)";

        /// <summary>
        /// 构建
        /// </summary>
        /// <param name="text"></param>
        public RuleDecorator Build(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return new EmptyRuleDecorator();
            }

            var matchCollection = Regex.Matches(text, LogicalOperator);

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
                equations[i] = text.Substring(start, count).Trim();
                start = match.Index + match.Length;
            }

            equations[equations.Length - 1] = text.Substring(start, text.Length - start).Trim();

            return new EmptyRuleDecorator();
        }
    }

    public class ExpressionResolverContext : ResponsibilityContext
    {
        public const char NotOperator = '!';
        /// <summary>
        /// 关系操作符
        /// </summary>
        public const string RelationalOperator = @"([><])|(>=)|(<=)|(==)";
        public ExpressionResolverContext(string expression)
        {
            this.Expression = expression;

            var operatorMatch = Regex.Match(this.Expression, RelationalOperator);

            if (operatorMatch.Success)
            {
                var left = this.Expression.Substring(0, operatorMatch.Index);
                var right = this.Expression.Substring(operatorMatch.Index + operatorMatch.Length);
                this.Left = left;
                this.Right = right;
            }
            else if (this.Expression.StartsWith(NotOperator))
            {
                this.Operator = NotOperator.ToString();
                this.Left = this.Expression.Substring(this.Operator.Length);
            }
        }
        /// <summary>
        /// 表达式
        /// </summary>
        /// <value></value>
        public string Expression { get; }
        /// <summary>
        /// 左值
        /// </summary>
        public string Left { get; }
        /// <summary>
        /// 操作符
        /// </summary>
        public string Operator { get; }
        /// <summary>
        /// 右值
        /// </summary>
        public string Right { get; }
        /// <summary>
        /// 规则装饰器
        /// </summary>
        /// <value></value>
        public RuleDecorator Decorator { get; private set; }
        /// <summary>
        /// 执行成功
        /// </summary>
        /// <param name="decorator"></param>
        public virtual void Success(RuleDecorator decorator)
        {
            this.Decorator = decorator;
            this.Success();
        }

        public override void Success()
        {
            SAE.CommonLibrary.Extension.Assert.Build(this.Decorator)
                                              .NotNull($"请设置'{nameof(this.Decorator)}'");
            base.Success();
        }
    }

    public abstract class ExpressionResolverResponsibility : IResponsibility<ExpressionResolverContext>
    {
        public virtual Task HandleAsync(ExpressionResolverContext context)
        {
            
        }

        protected abstract Task HandleCoreAsync(ExpressionResolverContext context);
    }

    public class NumberExpressionResolverResponsibility : ExpressionResolverResponsibility
    {
        protected override Task HandleCoreAsync(ExpressionResolverContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class StringExpressionResolverResponsibility : ExpressionResolverResponsibility
    {
        protected override Task HandleCoreAsync(ExpressionResolverContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolExpressionResolverResponsibility : ExpressionResolverResponsibility
    {
        protected override Task HandleCoreAsync(ExpressionResolverContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeExpressionResolverResponsibility : ExpressionResolverResponsibility
    {
        protected override Task HandleCoreAsync(ExpressionResolverContext context)
        {
            throw new NotImplementedException();
        }
    }
}