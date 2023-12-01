using System;
using System.Collections.Generic;
using System.Linq;
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
        internal RuleContext() : this(new Dictionary<string, string>())
        {
        }
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
        /// <summary>
        /// 合并<paramref name="context"/>
        /// </summary>
        /// <param name="context"></param>
        public void Merge(RuleContext context)
        {
            this.Merge(context._store);
        }
        /// <summary>
        /// 合并<paramref name="dict"/>
        /// </summary>
        /// <param name="dict"></param>
        public void Merge(IDictionary<string, string> dict)
        {
            if (dict == null) return;
            foreach (var kv in dict)
            {
                this._store[kv.Key.ToLower()] = kv.Value;
            }
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
        public override string ToString()
        {
            return this._store.ToJsonString();
        }
    }
}