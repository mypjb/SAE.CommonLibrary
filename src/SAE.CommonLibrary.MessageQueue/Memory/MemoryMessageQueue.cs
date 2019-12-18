using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using System;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.MessageQueue.Memory
{
    /// <summary>
    /// 基于内存的messageQueue
    /// </summary>
    public class MemoryMessageQueue : IMessageQueue
    {
        private readonly ILogging _logging;

        public MemoryMessageQueue(ILogging<MemoryMessageQueue> logging)
        {
            this._logging = logging;
            this._logging.Warn("您正在使用，基于内存的消息队列。在生产环境中请改用其他实现");
        }
        public void Dispose()
        {

        }

        public Task PublishAsync<TMessage>(string identity, TMessage message) where TMessage : class
        {
            this._logging.Info($"发布事件'{identity}' = > {message.ToJsonString()}");
            return this.HandlerCoreAsync(identity, message);
        }

        public Task SubscibeAcync<TMessage>(string identity, Func<TMessage, Task> @delegate) where TMessage : class
        {
            this._logging.Info($"订阅事件'{identity}' => {@delegate.Method.ToString()}");
            return this.MappingAsync(identity, @delegate);
        }
    }
}
