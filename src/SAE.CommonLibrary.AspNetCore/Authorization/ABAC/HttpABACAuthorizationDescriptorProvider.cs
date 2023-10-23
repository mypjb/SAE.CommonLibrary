using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SAE.CommonLibrary.Abstract.Decorator;
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
    /// 装饰器上下文
    /// </summary>
    public class RuleContext : DecoratorContext
    {
        private Queue<object> _queue { get; }
        private IDictionary<string, string> _store;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        public RuleContext(IDictionary<string, string> dict)
        {
            this._store = dict;
            this._queue = new Queue<object>();
        }
        public string Get(string key)
        {
            this._store.TryGetValue(key, out string val);
            return val;
        }
        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="o"></param>
        /// <summary>
        public void Enqueue(object o)
        {
            this._queue.Enqueue(o);
        }
        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T Dequeue<T>()
        {
            Assert.Build(this._queue.Any())
                  .True("队列已清空。");
            return (T)this._queue.Dequeue();
        }
    }
    /// <summary>
    /// 逻辑操作符装饰器
    /// </summary> 
    public class OperatorRuleDecorator : IDecorator<RuleContext>
    {
        public OperatorRuleDecorator(LogicalOperator @operator)
        {
            this.Operator = @operator;
        }
        private LogicalOperator Operator { get; }
        public async Task DecorateAsync(RuleContext context)
        {
            var left = context.Dequeue<bool>();
            var result = false;
            if (this.Operator == LogicalOperator.Not)
            {
                result = !left;
            }
            else if (this.Operator == LogicalOperator.None)
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


    /// <summary>
    /// 属性装饰器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc/>
    public class PropertyRuleDecorator<T> : IDecorator<RuleContext>
    {
        /// <summary>
        /// ctor
        /// </summary>
        protected PropertyRuleDecorator(string propertyName)
        {
            this.PropertyName = propertyName;
        }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; }

        public async Task DecorateAsync(RuleContext context)
        {
            var val = context.Get(this.PropertyName);
            var value = string.IsNullOrWhiteSpace(val) ? default(T) : val.ToObject<T>();
            context.Enqueue(value);
        }
    }
    /// <summary>
    /// 常量装饰器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConstantRuleDecorator<T> : IDecorator<RuleContext>
    {
        public ConstantRuleDecorator(T value)
        {
            this.Value = value;
        }
        public T Value { get; }

        public async Task DecorateAsync(RuleContext context)
        {
            context.Enqueue(this.Value);
        }
    }

    /// <summary>
    /// 一元操作符装饰器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc/>
    public class BinaryRuleDecorator<T> : IDecorator<RuleContext> where T : IComparable, IEquatable<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public RelationalOperator Operator { get; }
        /// <summary>
        /// 
        /// </summary>
        public BinaryRuleDecorator(RelationalOperator @operator)
        {
            this.Operator = @operator;
        }

        public async Task DecorateAsync(RuleContext context)
        {
            var left = context.Dequeue<T>();
            var right = context.Dequeue<T>();
            bool result = false;
            switch (this.Operator)
            {
                case RelationalOperator.GreaterThan:
                    {
                        result = left.CompareTo(right) > 0;
                        break;
                    }
                case RelationalOperator.LessThan:
                    {
                        result = right.CompareTo(right) < 0;
                        break;
                    }
                case RelationalOperator.Equal:
                    {
                        result = right.CompareTo(right) == 0;
                        break;
                    }
                case RelationalOperator.NotEqual:
                    {
                        result = right.CompareTo(right) != 0;
                        break;
                    }
                case RelationalOperator.GreaterThanOrEqual:
                    {
                        result = right.CompareTo(right) >= 0;
                        break;
                    }
                case RelationalOperator.LessThanOrEqual:
                    {
                        result = right.CompareTo(right) <= 0;
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

            context.Enqueue(result);
        }

    }
}