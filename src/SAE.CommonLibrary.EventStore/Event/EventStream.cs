using SAE.CommonLibrary.EventStore.Event;
using SAE.CommonLibrary.EventStore.Serialize;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SAE.CommonLibrary.EventStore
{
    /// <summary>
    /// 事件流
    /// </summary>
    public class EventStream : IEnumerable<IEvent>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="version"></param>
        /// <param name="eventJsons"></param>
        /// <param name="dateTime"></param>
        public EventStream(IIdentity identity, int version, string eventJsons, DateTimeOffset dateTime) : this(identity, version, Enumerable.Empty<IEvent>(), dateTime)
        {
            var serializer = SerializerProvider.Current;

            this._store = serializer.Deserialize<List<WrapperEvent>>(eventJsons)
                                    .Cast<IEvent>()
                                    .ToList();
        }

        /// <summary>
        /// 初始化一个事件流
        /// </summary>
        /// <param name="identity">主键</param>
        /// <param name="version">版本号</param>
        /// <param name="events">事件集合</param>
        public EventStream(IIdentity identity, int version, IEnumerable<IEvent> events) : this(identity, version, events, DateTimeOffset.Now)
        {
        }

        /// <summary>
        /// 初始化一个事件流
        /// </summary>
        /// <param name="identity">主键</param>
        /// <param name="version">版本号</param>
        /// <param name="events">事件集合</param>
        /// <param name="timeStamp">时间戳</param>
        public EventStream(IIdentity identity, int version, IEnumerable<IEvent> events, DateTimeOffset timeStamp)
        {
            this._store = new List<IEvent>();
            this.Identity = identity;
            this.Version = version;
            if (events != null)
            {
                foreach (var @event in events)
                {
                    this._store.Add(new WrapperEvent(@event));
                }
            }

            this.TimeStamp = timeStamp;
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// 标识
        /// </summary>
        public IIdentity Identity { get; private set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTimeOffset TimeStamp { get; private set; }

        /// <summary>
        /// 事件存储
        /// </summary>
        private readonly List<IEvent> _store;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IEvent> GetEnumerator() => this.Recover();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        ///  将内部事件还原为外部事件
        /// </summary>
        /// <returns></returns>
        protected IEnumerator<IEvent> Recover()
        {
            //var logging = ServiceFacade.GetService<ILogging<EventStream>>();

            //List<IEvent> eventList = new List<IEvent>();

            //this._store.ForEach(e =>
            //{
            //    var internalEvent = e as WrapperEvent;
            //    var type = internalEvent.GetEventType();
            //    logging.Debug($"Recover type:'{type}',:'{internalEvent.Event}',raw:'{internalEvent.ToJsonString()}'");
            //    var @event = (IEvent)SerializerProvider.Current.Deserialize(internalEvent.Event, type);
            //    eventList.Add(@event);
            //});

            return this._store.GetEnumerator();
        }

        /// <summary>
        /// 附加事件,该操作将会替换当前<see cref="EventStream.Identity"/>,<see cref="EventStream.TimeStamp"/>,<see cref="EventStream.Version"/>并附加event
        /// </summary>
        /// <param name="eventStream"></param>
        public void Append(EventStream eventStream)
        {
            this.Identity = eventStream.Identity;
            this.TimeStamp = eventStream.TimeStamp;
            this.Version = eventStream.Version;
            this._store.AddRange(eventStream._store);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        public void Valid(int version)
        {
            Assert.Build(this.Version >= version)
                  .False("当前版本过低");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return SerializerProvider.Current
                                     .Serialize(this._store);
        }
    }
}
