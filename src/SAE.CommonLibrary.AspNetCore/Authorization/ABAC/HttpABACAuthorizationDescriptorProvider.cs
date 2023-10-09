using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    public class HttpABACAuthorizationDescriptorProvider : IABACAuthorizationDescriptorProvider
    {
        public HttpABACAuthorizationDescriptorProvider()
        {

        }

        public Task<IABACAuthorizationDescriptor> GetAsync()
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 关系运算符
    /// </summary>
    public enum RelationalOperator
    {
        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan,
        /// <summary>
        /// 小于
        /// </summary>
        LessThan,
        /// <summary>
        /// 等于
        /// </summary>
        Equal,
        ///<summary>
        /// 不等于
        ///</summary>
        NotEqual,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessThanOrEqual,
        /// <summary>
        /// 包含
        /// </summary>
        Include,
        /// <summary>
        /// 正则表达式
        /// </summary>
        Regex
    }
    /// <summary>
    /// 规则
    /// </summary>
    public class Rule
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 操作符合
        /// </summary>
        /// <value></value>
        public LogicalOperator Operator { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        /// <value></value>
        public string Value { get; set; }
    }
    /// <summary>
    /// 逻辑运算符
    /// </summary>
    public enum LogicalOperator
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        /// <summary>
        /// 逻辑与
        /// </summary>
        And,
        /// <summary>
        /// 逻辑或
        /// </summary>
        Or,
        /// <summary>
        /// 否定
        /// </summary>
        Not
    }

    /// <summary>
    /// 
    /// </summary>
    public class RuleContext
    {
        private IDictionary<string, string> _store;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        public RuleContext(IDictionary<string, string> dict)
        {
            this._store = dict;
        }
        public string Get(string key)
        {
            this._store.TryGetValue(key, out string val);
            return val;
        }
    }
    /// <summary>
    /// 
    /// </summary> 
    public abstract class RuleDecorator
    {
        private RuleDecorator Decorator;
        /// <summary>
        /// ctor
        /// </summary>
        public RuleDecorator()
        {

        }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="operator"></param>
        /// <param name="decorator"></param>
        public RuleDecorator(LogicalOperator @operator, RuleDecorator decorator)
        {
            this.Operator = @operator;
            decorator.Set(this);
        }

        /// <summary>
        /// operator
        /// </summary>
        /// <value></value>
        public LogicalOperator Operator { get; private set; }
        /// <summary>
        /// 设置<see cref="Decorator"/>
        /// </summary>
        /// <param name="decorator"></param>
        public void Set(RuleDecorator decorator)
        {
            this.Decorator = decorator;
            this.Operator = decorator.Operator;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        public virtual bool Execute(RuleContext ctx)
        {
            var left = this.ExecuteCore(ctx);

            if (this.Decorator != null)
            {
                switch (this.Operator)
                {
                    case LogicalOperator.And:
                        {
                            if (left)
                            {
                                left = this.Decorator.Execute(ctx);
                            }
                            break;
                        }
                    case LogicalOperator.Or:
                        {
                            if (!left)
                            {
                                left = this.Decorator.Execute(ctx);
                            }
                            break;
                        }
                    case LogicalOperator.Not:
                        {
                            left = !left;
                            break;
                        }
                }
            }
            return left;
        }
        /// <summary>
        /// execute core
        /// </summary>
        /// <param name="ctx"></param>
        protected abstract bool ExecuteCore(RuleContext ctx);
    }

    /// <summary>
    /// 空的装饰器
    /// </summary>
    public class EmptyRuleDecorator : RuleDecorator
    {
        protected override bool ExecuteCore(RuleContext ctx)
        {
            return false;
        }
    }

    /// <summary>
    /// 属性装饰器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc/>
    public abstract class PropertyRuleDecorator<T> : RuleDecorator
    {
        /// <summary>
        /// ctor
        /// </summary>
        protected PropertyRuleDecorator()
        {
        }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="operator"></param>
        /// <param name="decorator"></param>
        protected PropertyRuleDecorator(LogicalOperator @operator, RuleDecorator decorator) : base(@operator, decorator)
        {
        }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; protected set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public T Value { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual T Get(RuleContext ctx)
        {
            var val = ctx.Get(this.PropertyName);
            return string.IsNullOrWhiteSpace(val) ? default(T) : val.ToObject<T>();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc/>
    public class BinaryRuleDecorator<T> : PropertyRuleDecorator<T> where T : IComparable, IEquatable<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public RelationalOperator RelationalOperator { get; }
        /// <summary>
        /// 
        /// </summary>
        public BinaryRuleDecorator()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operator"></param>
        /// <param name="decorator"></param>
        public BinaryRuleDecorator(LogicalOperator @operator, RuleDecorator decorator) : base(@operator, decorator)
        {
        }

        protected override bool ExecuteCore(RuleContext ctx)
        {
            var right = this.Get(ctx);

            var result = false;

            switch (this.RelationalOperator)
            {
                case RelationalOperator.GreaterThan:
                    {
                        result = Value.CompareTo(right) > 0;
                        break;
                    }
                case RelationalOperator.LessThan:
                    {
                        result = Value.CompareTo(right) < 0;
                        break;
                    }
                case RelationalOperator.Equal:
                    {
                        result = Value.CompareTo(right) == 0;
                        break;
                    }
                case RelationalOperator.NotEqual:
                    {
                        result = Value.CompareTo(right) != 0;
                        break;
                    }
                case RelationalOperator.GreaterThanOrEqual:
                    {
                        result = Value.CompareTo(right) >= 0;
                        break;
                    }
                case RelationalOperator.LessThanOrEqual:
                    {
                        result = Value.CompareTo(right) <= 0;
                        break;
                    }
                case RelationalOperator.Include:
                    {
                        var left = this.Value?.ToString();
                        var r = right?.ToString();
                        result = left == null || r == null ? false : left.IndexOf(r) != -1;
                        break;
                    }
                case RelationalOperator.Regex:
                    {
                        var left = this.Value?.ToString();
                        var r = right?.ToString() ?? string.Empty;
                        result = string.IsNullOrWhiteSpace(left) ? false : Regex.Match(left, r).Success;
                        break;
                    }
            }
            return result;
        }

    }


}