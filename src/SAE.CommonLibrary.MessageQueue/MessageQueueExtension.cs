using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.DependencyInjection;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.MessageQueue
{
    /// <summary>
    /// 
    /// </summary>
    public static class MessageQueueExtension
    {
        private readonly static ConcurrentDictionary<string, Delegate> _concurrentDictionary;
        static MessageQueueExtension()
        {
            _concurrentDictionary = new ConcurrentDictionary<string, Delegate>();
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messageQueue"></param>
        /// <param name="message">消息主体</param>
        public static Task PublishAsync<TMessage>(this IMessageQueue messageQueue, TMessage message) where TMessage : class
        {
            var identity = Utils.Get(message);
            return messageQueue.PublishAsync(identity, message);
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messageQueue"></param>
        /// <param name="message">消息主体</param>
        public static void Publish<TMessage>(this IMessageQueue messageQueue, TMessage message) where TMessage : class
        {
            var identity = Utils.Get(message);
            messageQueue.Publish(identity, message);
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messageQueue"></param>
        /// <param name="identity">发布标识</param>
        /// <param name="message">消息主体</param>
        public static void Publish<TMessage>(this IMessageQueue messageQueue, string identity, TMessage message) where TMessage : class
        {
            messageQueue.PublishAsync(identity, message)
                        .GetAwaiter()
                        .GetResult();
        }

        /// <summary>
        /// 使用<typeparamref name="TMessage"/>类型作为订阅identity
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messageQueue"></param>
        /// <param name="delegate">订阅处理程序</param>
        /// <returns></returns>
        public static void Subscibe<TMessage>(this IMessageQueue messageQueue, Func<TMessage, Task> @delegate) where TMessage : class
        {
            messageQueue.SubscibeAsync(@delegate)
                        .GetAwaiter()
                        .GetResult();
        }

        /// <summary>
        /// 使用<typeparamref name="TMessage"/>类型作为订阅identity
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        public static void Subscibe<TMessage>(this IMessageQueue messageQueue) where TMessage : class
        {
            messageQueue.SubscibeAsync<TMessage>()
                        .GetAwaiter()
                        .GetResult();
        }

        /// <summary>
        /// 使用<typeparamref name="TMessage"/>类型作为订阅identity
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messageQueue"></param>
        /// <param name="delegate">订阅处理程序</param>
        /// <returns></returns>
        public static async Task SubscibeAsync<TMessage>(this IMessageQueue messageQueue, Func<TMessage, Task> @delegate) where TMessage : class
        {
            var identity = Utils.Get<TMessage>();
            await messageQueue.MappingAsync(identity, @delegate);
            await messageQueue.SubscibeAsync(identity, @delegate);
        }

        /// <summary>
        /// 使用<typeparamref name="TMessage"/>类型作为订阅identity,并使用依赖注入的方式进行订阅处理
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messageQueue"></param>
        public static async Task SubscibeAsync<TMessage>(this IMessageQueue messageQueue) where TMessage : class
        {
            var identity = Utils.Get<TMessage>();
            await messageQueue.SubscibeAsync<TMessage>(message =>
            {
                var handler = ServiceFacade.GetService<IHandler<TMessage>>();
                return handler.HandleAsync(message);
            });
        }

        /// <summary>
        /// 添加对事件<paramref name="identity"/>的映射。
        /// </summary>
        /// <param name="messageQueue"></param>
        /// <param name="identity">标识</param>
        /// <param name="delegate">执行的委托</param>
        /// <returns></returns>
        public static Task MappingAsync<TMessage>(this IMessageQueue messageQueue, string identity, Func<TMessage, Task> @delegate) where TMessage : class
        {
            _concurrentDictionary.AddOrUpdate(identity, @delegate, (a, b) =>
             {
                 return @delegate;
             });

            return Task.CompletedTask;
        }


        /// <summary>
        /// 执行订阅程序的处理
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messageQueue"></param>
        /// <param name="identity">订阅标识</param>
        /// <param name="message">订阅消息</param>
        /// <returns></returns>
        public static async Task HandlerCoreAsync<TMessage>(this IMessageQueue messageQueue,
                                                            string identity,
                                                            TMessage message) where TMessage : class
        {
            var logging = ServiceFacade.GetService<ILogging<TMessage>>();
            Delegate @delegate;
            if (_concurrentDictionary.TryGetValue(identity, out @delegate))
            {
                logging.Debug($"找到'{identity}'订阅的处理程序");
                var func = @delegate as Func<TMessage, Task>;
                if (func != null)
                {
                    try
                    {
                        await func.Invoke(message);
                    }
                    catch (Exception ex)
                    {
                        logging.Error("消息执行失败!", ex);
                        throw ex;
                    }
                }
                else
                {
                    logging.Error($"'{identity}'订阅的程序不是Func<TMessage, Task>委托，执行失败!");
                }
            }
            else
            {
                logging.Warn($"未找到'{identity}'订阅对应的处理程序");
            }
        }

        /// <summary>
        /// 执行订阅程序的处理
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messageQueue"></param>
        /// <param name="message">订阅消息</param>
        /// <returns></returns>
        public static Task HandlerCoreAsync<TMessage>(this IMessageQueue messageQueue, TMessage message) where TMessage : class
        {
            var identity = Utils.Get(message);
            return messageQueue.HandlerCoreAsync(identity, message);
        }
    }
}
