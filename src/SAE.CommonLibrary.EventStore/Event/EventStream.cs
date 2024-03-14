using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SAE.CommonLibrary.EventStore.Serialize;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.EventStore
{
    /// <summary>
    /// 事件流
    /// </summary>
    /// <inheritdoc/>
    public class EventStream : IEnumerable<string>
    {
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="identity">标识</param>
        /// <param name="version">版本号</param>
        /// <param name="eventString">事件序列化后的数据</param>
        public EventStream(IIdentity identity, int version, string eventString) : this(identity, version, eventString, DateTimeOffset.UtcNow)
        {
        }
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="identity">标识</param>
        /// <param name="version">版本号</param>
        /// <param name="eventString">事件序列化后的数据</param>
        /// <param name="dateTime">事件流发生的时间</param>
        public EventStream(IIdentity identity, int version, string eventString, DateTimeOffset dateTime)
        {
            this._store = new List<string>();
            this.Identity = identity;
            this.Version = version;
            this.Timestamp = dateTime;

            if (!eventString.IsNullOrWhiteSpace())
            {
                this._store.Add(eventString);
                this.Data = eventString;
            }
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
        public DateTimeOffset Timestamp { get; private set; }
        /// <summary>
        /// 事件的字符串形式
        /// </summary>
        /// <value></value>
        public string Data { get; set; }

        /// <summary>
        /// 事件存储
        /// </summary>
        private readonly List<string> _store;
        ///<inheritdoc/>
        public IEnumerator<string> GetEnumerator() => this._store.GetEnumerator();

        ///<inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// 附加事件,该操作将会替换当前<see cref="EventStream.Identity"/>,<see cref="EventStream.Timestamp"/>,<see cref="EventStream.Version"/>并附加event
        /// </summary>
        /// <param name="eventStream">事件流</param>
        public void Append(EventStream eventStream)
        {
            this.Identity = eventStream.Identity;
            this.Timestamp = eventStream.Timestamp;
            this.Version = eventStream.Version;

            if (eventStream._store.Any())
                this._store.AddRange(eventStream._store);
        }

        /// <summary>
        /// 验证版本号
        /// </summary>
        /// <param name="version">版本号</param>
        public void Valid(int version)
        {
            Assert.Build(this.Version >= version)
                  .False("当前版本过低");
        }
    }
}
