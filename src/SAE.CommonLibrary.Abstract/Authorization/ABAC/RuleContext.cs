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
}