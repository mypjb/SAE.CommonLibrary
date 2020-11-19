using Microsoft.Extensions.Options;
using SAE.CommonLibrary.EventStore.Serialize;
using SAE.CommonLibrary.EventStore.Snapshot;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.EventStore.Document
{
    /// <summary>
    /// 默认的文档存储对象
    /// </summary>
    public class DefaultDocumentStore : IDocumentStore
    {
        private readonly ISnapshotStore _snapshot;
        private readonly ISerializer _serializer = SerializerProvider.Current;
        private readonly IEventStore _eventStore;
        private readonly IEnumerable<IDocumentEvent> _documentEvents;
        private readonly DocumentOptions _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snapshot"></param>
        /// <param name="eventStore"></param>
        /// <param name="documentEvents"></param>
        public DefaultDocumentStore(ISnapshotStore snapshot,
                                    IEventStore eventStore,
                                    IEnumerable<IDocumentEvent> documentEvents,
                                    IOptions<DocumentOptions> options)
        {
            this._snapshot = snapshot;
            this._eventStore = eventStore;
            this._documentEvents = documentEvents;
            this._options = options.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="identity"></param>
        /// <param name="version"></param>
        /// <returns></returns>
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
            foreach (IEvent @event in eventStream)
            {
                document.Mutate(@event);
            }
            document.Version = eventStream.Version <= 0 ? snapshot.Version : eventStream.Version;
            return document;
        }

        /// <summary>
        /// 查找快照
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="version"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public virtual async Task SaveAsync<TDocument>(TDocument document) where TDocument : IDocument, new()
        {
            var identity = document.Identity;

            var currentVersion = await this._eventStore.GetVersionAsync(identity);

            if (currentVersion > document.Version)
            {
                throw new Exception("版本不一致");
            }
            //将当前版本号+1以保持循序性
            var version = currentVersion + 1;
            document.Version = version;
            //创建事件流
            var eventStream = new EventStream(identity, version, document.ChangeEvents);
            //累加事件流
            await this._eventStore.AppendAsync(eventStream);

            //附加事件
            await this._documentEvents.ForEachAsync(@event => @event.AppendAsync(document, eventStream));
            
            //如果版本号满足快照要求就将对象存储到快照中
            if (version % this._options.SnapshotInterval != 0)
            {
                //不满足快照要求
                return;
            }

            ////从快照存储获取对应快照
            //var snapshot = await this._snapshot.FindAsync(identity);
            ////反序列化文档
            //document = this._serializer.Deserialize(snapshot.Data, Type.GetType(snapshot.Type)) as IDocument;
            ////重放事件
            //foreach (IEvent @event in eventStream)
            //{
            //    document.Mutate(@event);
            //}
            //
            await this._snapshot.SaveAsync(new Snapshot.Snapshot(identity, document, document.Version));
        }

        /// <summary>
        /// 根据<paramref name="identity"/>移除文档对象
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public async Task DeleteAsync<TDocument>(IIdentity identity) where TDocument : IDocument, new()
        {
            await this._snapshot.RemoveAsync(identity);
            await this._eventStore.RemoveAsync(identity);
            await this._documentEvents.ForEachAsync(@event => @event.RemoveAsync<TDocument>(identity));
        }
    }
}
