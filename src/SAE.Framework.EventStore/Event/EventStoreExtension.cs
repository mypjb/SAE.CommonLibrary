using SAE.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.EventStore
{
    /// <summary>
    /// 事件存储扩展
    /// </summary>
    public static class EventStoreExtension
    {

        /// <summary>
        /// 加载事件流
        /// </summary>
        /// <param name="store">事件存储</param>
        /// <param name="identity">标识</param>
        /// <returns>事件流</returns>
        public static EventStream LoadEventStream(this IEventStore store,IIdentity identity)
        {
            return store.LoadEventStream(identity, 0, int.MaxValue);
        }

        /// <summary>
        /// 加载事件流
        /// </summary>
        /// <param name="store">事件存储</param>
        /// <param name="identity">标识</param>
        /// <param name="skipEvent">跳过事件</param>
        /// <param name="maxCount">返回事件的长度</param>
        /// <returns>事件流</returns>
        public static EventStream LoadEventStream(this IEventStore store, IIdentity identity,int skipEvent,int maxCount)
        {
            var task = store.LoadEventStreamAsync(identity, skipEvent, maxCount);
            
            return task.GetAwaiter()
                       .GetResult();
        }
        /// <summary>
        /// 附加事件
        /// </summary>
        /// <param name="store">事件存储</param>
        /// <param name="eventStream">要附加的事件流</param>
        public static void Append(this IEventStore store, EventStream eventStream)
        {
            store.AppendAsync(eventStream)
                 .Wait();
        }
        /// <summary>
        /// 使用<paramref name="identity"/>返回当前版本号
        /// </summary>
        /// <param name="store">事件存储</param>
        /// <param name="identity">事件流标识</param>
        /// <returns>版本号</returns>
        public static long GetVersion(this IEventStore store, IIdentity identity)
        {
            return store.GetVersionAsync(identity)
                        .GetAwaiter()
                        .GetResult();
        }

        /// <summary>
        /// 获得事件的标识
        /// </summary>
        /// <param name="event">事件</param>
        /// <returns>标识</returns>
        internal static string GetKey(this IEvent @event)
        {
            var type = @event.GetType();
            return type.GetIdentity();
        }
    }
}
