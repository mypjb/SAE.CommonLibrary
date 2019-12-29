using SAE.CommonLibrary.EventStore.Serialize;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore.Event
{
    /// <summary>
    /// 内部事件包装对象
    /// </summary>
    internal class InternalEvent:IEvent
    {
        public InternalEvent()
        {

        }
        public InternalEvent(IEvent @event)
        {
            Event = SerializerProvider.Current
                                      .Serialize(@event);
            var type = @event.GetType();
            this.Type = $"{type.FullName},{type.Assembly.GetName().Name}";
        }
        /// <summary>
        /// 事件类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 事件
        /// </summary>
        public string Event { get; set; }


        public Type GetEventType()
        {
            return System.Type.GetType(Type);
        }
    }
}
