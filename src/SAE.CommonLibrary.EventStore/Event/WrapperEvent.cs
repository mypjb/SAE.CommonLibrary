using SAE.CommonLibrary.EventStore.Serialize;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore.Event
{
    /// <summary>
    /// 内部事件包装对象
    /// </summary>
    public class WrapperEvent:IEvent
    {
        public WrapperEvent()
        {

        }
        public WrapperEvent(IEvent @event)
        {
            Event = SerializerProvider.Current
                                      .Serialize(@event);
            this.Key = @event.GetKey();
        }
        /// <summary>
        /// 事件类型
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 事件
        /// </summary>
        public string Event { get; set; }


        public Type GetEventType()
        {
            var mapping= ServiceFacade.GetService<IEventMapping>();

            return mapping.Get(this.Key);
        }
    }
}
