using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.MessageQueue
{
    /// <summary>
    /// 消息队列接口
    /// </summary>
    public interface IMessageQueue : IDisposable
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="identity">发布标识</param>
        /// <param name="message">消息主体</param>
        Task PublishAsync<TMessage>(string identity,TMessage message) where TMessage : class;

        /// <summary>
        /// 根据<paramref name="identity"/>订阅事件
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="identity">消息标识</param>
        /// <param name="delegate">订阅委托处理程序</param>
        Task SubscribeAsync<TMessage>(string identity, Func<TMessage, Task> @delegate) where TMessage : class;
    }
}
