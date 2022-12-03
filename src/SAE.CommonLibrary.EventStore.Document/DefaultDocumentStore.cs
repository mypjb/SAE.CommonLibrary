using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.EventStore.Serialize;
using SAE.CommonLibrary.EventStore.Snapshot;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.EventStore.Document
{
    /// <summary>
    /// 默认的文档存储对象
    /// </summary>
    /// <inheritdoc/>
    public class DefaultDocumentStore : IDocumentStore
    {
        private readonly ISnapshotStore _snapshot;
        private readonly ISerializer _serializer;
        private readonly IEventStore _eventStore;
        private readonly IEnumerable<IDocumentEvent> _documentEvents;
        private readonly ILogging<DefaultDocumentStore> _logging;
        private readonly DocumentOptions _options;

        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="snapshot">快照存储接口</param>
        /// <param name="eventStore">事件存储接口</param>
        /// <param name="documentEvents">文档事件集合</param>
        /// <param name="serializer">事件序列化器</param>
        /// <param name="options">文档配置</param>
        /// <param name="mapping">事件映射器</param>
        /// <param name="logging">日志记录器</param>
        public DefaultDocumentStore(ISnapshotStore snapshot,
                                    IEventStore eventStore,
                                    IEnumerable<IDocumentEvent> documentEvents,
                                    ISerializer serializer,
                                    IOptions<DocumentOptions> options,
                                    ILogging<DefaultDocumentStore> logging)
        {
            this._snapshot = snapshot;
            this._eventStore = eventStore;
            this._documentEvents = documentEvents;
            this._serializer = serializer;
            this._logging = logging;
            this._options = options.Value;
        }
        public virtual async Task<TDocument> FindAsync<TDocument>(IIdentity identity, int version) where TDocument : IDocument, new()
        {
            //获取快照
            var snapshot = await this.FindSnapshotAsync(identity, version) ?? new Snapshot.Snapshot(identity);
            //加载事件流
            var eventStream = await this._eventStore.LoadEventStreamAsync(identity, snapshot.Version, version - snapshot.Version);
            //序列化文档
            TDocument document;
            if (snapshot.Version <= 0)
            {
                if (eventStream == null || eventStream.Version <= 0)
                {
                    return default;
                }
                document = new TDocument();
            }
            else
            {
                document = this._serializer.Deserialize<TDocument>(snapshot.Data);
            }

            //重放事件
            foreach (var eventStrings in eventStream)
            {
                await this.AppendEventAsync(document, eventStrings);
            }
            document.Version = eventStream.Version <= 0 ? snapshot.Version : eventStream.Version;
            return document;
        }
        /// <summary>
        /// 将<paramref name="eventStrings"/>附加到<paramref name="document"/>
        /// </summary>
        /// <param name="document">文档对象</param>
        /// <param name="eventStrings">事件字符串</param>
        protected virtual async Task AppendEventAsync(IDocument document, string eventStrings)
        {
            var events = this._serializer.Deserialize<object[]>(eventStrings);
            foreach (var @event in events)
            {
                this._serializer.Deserialize(@event.ToString(), document);
            }
        }

        /// <summary>
        /// 查找快照
        /// </summary>
        /// <param name="identity">标识</param>
        /// <param name="version">版本号</param>
        /// <returns></returns>
        protected virtual async Task<Snapshot.Snapshot> FindSnapshotAsync(IIdentity identity, int version)
        {
            if (version == this._options.VersionPeak)
            {
                return await this._snapshot.FindAsync(identity);
            }

            var snapshotInterval = this._options.SnapshotInterval;

            if (version < snapshotInterval)
            {
                return new Snapshot.Snapshot();
            }

            var snapshotVersion = version - version % snapshotInterval;

            var snapshot = await this._snapshot.FindAsync(identity, snapshotVersion);

            return snapshot;
        }

        public virtual async Task SaveAsync<TDocument>(TDocument document) where TDocument : IDocument, new()
        {
            var identity = document.Identity;

            var currentVersion = await this._eventStore.GetVersionAsync(identity);

            if (currentVersion > document.Version)
            {
                throw new SAEException(StatusCodes.Custom, "版本不一致");
            }
            //将当前版本号+1以保持循序性
            var version = currentVersion + 1;
            document.Version = version;
            //创建事件流
            var eventStream = new EventStream(identity, version, this._serializer.Serialize(document.ChangeEvents));

            if (eventStream.Count() == 0)
            {
                this._logging.Warn("事件内不存在数据，丢弃更改！");
                return;
            }

            if (eventStream.Count() != 1)
            {
                this._logging.Warn($"事件流存储的时候，有且只有一个事件流，不允许存在多个!当前存在'{eventStream.Count()}'个");
                return;
            }


            //累加事件流
            await this._eventStore.AppendAsync(eventStream);

            //触发附加事件
            await this._documentEvents.ForEachAsync(async @event => await @event.AppendAsync(document, document.ChangeEvents));

            //如果版本号满足快照要求就将对象存储到快照中
            if (version % this._options.SnapshotInterval != 0)
            {
                //不满足快照要求
                return;
            }

            await this._snapshot.SaveAsync(new Snapshot.Snapshot(identity, this._serializer.Serialize(document), document.Version));
        }

        public async Task DeleteAsync<TDocument>(IIdentity identity) where TDocument : IDocument, new()
        {
            await this._snapshot.DeleteAsync(identity);
            await this._eventStore.DeleteAsync(identity);
            await this._documentEvents.ForEachAsync(async @event => await @event.DeleteAsync<TDocument>(identity));
        }
    }
}
