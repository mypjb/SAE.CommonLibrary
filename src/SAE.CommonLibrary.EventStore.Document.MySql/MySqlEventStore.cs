using System;
using System.Threading.Tasks;
using Dapper;
using SAE.CommonLibrary.Database;

namespace SAE.CommonLibrary.EventStore.Document.MySql
{
    /// <summary>
    /// 基于内存的事件存储
    /// </summary>
    public class MySqlEventStore : IEventStore
    {
        private readonly IDBConnectionFactory _factory;

        public MySqlEventStore(IDBConnectionFactory factory)
        {
            this._factory = factory;
        }
        public async Task AppendAsync(EventStream eventStream)
        {
            using (var conn = await this._factory.GetAsync())
            {
                if (await conn.ExecuteAsync("insert into event_stream(id,timestamp,version,data) values(@id,@timestamp,@version,@data)", new
                {
                    Id = eventStream.Identity.ToString(),
                    Timestamp = eventStream.TimeStamp,
                    eventStream.Version,
                    Data = eventStream.ToString()
                }) != 1)
                {
                    throw new SAEException(StatusCodes.Custom, "event stream append fail");
                }
            }
        }

        public async Task<int> GetVersionAsync(IIdentity identity)
        {
            using (var conn = await this._factory.GetAsync())
            {
                return await conn.ExecuteScalarAsync<int>("select version from event_stream where id=@id order by version desc limit 1", new { id = identity.ToString() });
            }
        }

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

        public async Task DeleteAsync(IIdentity identity)
        {
            using (var conn = await this._factory.GetAsync())
            {
                await conn.ExecuteAsync("delete from  event_stream where id=@id", new { id = identity.ToString() });
            }
        }
    }
}
