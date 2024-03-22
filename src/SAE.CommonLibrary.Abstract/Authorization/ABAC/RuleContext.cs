using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAE.CommonLibrary.Abstract.Decorator;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 装饰器上下文
    /// </summary>
    public class RuleContext : DecoratorContext
    {
        /// <summary>
        /// ctor
        /// </summary>
        internal RuleContext() : this(new Dictionary<string, string>())
        {
        }
        /// <summary>
        /// 队列
        /// </summary>
        private Queue<object> _queue { get; }
        /// <summary>
        /// 字典
        /// </summary>
        /// <remarks>
        /// 存储属性的key字符表示。
        /// </remarks>
        private IDictionary<string, string> _store;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dict">属性字典</param>
        public RuleContext(IDictionary<string, string> dict)
        {
            this._store = dict;
            this._queue = new Queue<object>();
        }
        /// <summary>
        /// 合并<paramref name="context"/>
        /// </summary>
        /// <param name="context">待合并的上下文</param>
        public void Merge(RuleContext context)
        {
            this.Merge(context._store);
        }
        /// <summary>
        /// 合并<paramref name="dict"/>
        /// </summary>
        /// <param name="dict">属性字典</param>
        public void Merge(IDictionary<string, string> dict)
        {
            if (dict == null || dict.Count == 0) return;
            foreach (var kv in dict)
            {
                this._store[kv.Key.ToLower()] = kv.Value;
            }
        }
        /// <summary>
        /// 获得value
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>属性的字符表示</returns>
        public string Get(string key)
        {
            this._store.TryGetValue(key.ToLower(), out string val);
            return val;
        }
        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="o">待入队的对象</param>
        public void Enqueue(object o)
        {
            this._queue.Enqueue(o);
        }
        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T">出队类型</typeparam>
        /// <returns>出队对象</returns>
        public T Dequeue<T>()
        {
            Assert.Build(this._queue.Any())
                  .True("队列已清空。");
            return (T)this._queue.Dequeue();
        }
        /// <summary>
        /// 以字符串的形式进行显示
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var kv in this._store.OrderBy(s => s.Key))
            {
                sb.Append(kv.Key).Append(":").Append(kv.Value).Append(",");
            }
            return sb.ToString();
        }
    }
}