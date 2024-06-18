using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SAE.Framework.Database;
using SAE.Framework.EventStore.Serialize;

namespace SAE.Framework.EventStore.Document.MySQL
{
    /// <summary>
    /// 基于mysql的事件存储
    /// </summary>
    /// <inheritdoc/>
    public class MySqlEventStore : IEventStore
    {
        private readonly IDBConnectionFactory _factory;
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="factory">数据库连接工厂</param>
        public MySqlEventStore(IDBConnectionFactory factory)
        {
            this._factory = factory;
        }
        /// <inheritdoc/>
        public async Task AppendAsync(EventStream eventStream)
        {
            using (var conn = await this._factory.GetAsync())
            {
                if (await conn.ExecuteAsync("insert into event_stream(id,timestamp,version,data) values(@id,@timestamp,@version,@data)", new
                {
                    Id = eventStream.Identity.ToString(),
                    eventStream.Timestamp,
                    eventStream.Version,
                    eventStream.Data
                }) != 1)
                {
                    throw new SAEException(StatusCodes.Custom, "event stream append fail");
                }
            }
        }
        /// <inheritdoc/>
        public async Task<int> GetVersionAsync(IIdentity identity)
        {
            using (var conn = await this._factory.GetAsync())
            {
                return await conn.ExecuteScalarAsync<int>("select version from event_stream where id=@id order by version desc limit 1", new { id = identity.ToString() });
            }
        }
        /// <inheritdoc/>
        public async Task<EventStream> LoadEventStreamAsync(IIdentity identity, int skipEvents, int maxCount)
        {
            var eventStream = new EventStream(identity, 0, string.Empty);
            using (var conn = await this._factory.GetAsync())
            {
                using (var reader = await conn.ExecuteReaderAsync($"select * from event_stream where id=@id and version > @skipVersion limit {maxCount}",
                                                                 new
                                                                 {
                                                                     Id = identity.ToString(),
                                                                     SkipVersion = skipEvents
                                                                 }))
                {
                    while (reader.Read())
                    {
                        eventStream.Append(new EventStream(identity,
                                           reader.GetInt32(reader.GetOrdinal("version")),
                                           reader.GetString(reader.GetOrdinal("data")),
                                           reader.GetDateTime(reader.GetOrdinal("timestamp"))));
                    }
                }
            }
            return eventStream;
        }
        /// <inheritdoc/>
        public async Task DeleteAsync(IIdentity identity)
        {
            using (var conn = await this._factory.GetAsync())
            {
                await conn.ExecuteAsync("delete from  event_stream where id=@id", new { id = identity.ToString() });
            }
        }
    }
}
