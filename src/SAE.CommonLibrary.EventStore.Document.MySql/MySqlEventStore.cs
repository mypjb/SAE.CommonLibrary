using Dapper;
using SAE.CommonLibrary.Database;
using System;
using System.Threading.Tasks;

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
            using (var conn= await this._factory.GetAsync())
            {
                if (await conn.ExecuteAsync("insert into EventStream(id,timestamp,version,data) values(@id,@timestamp,@version,@data)", new
                {
                    Id = eventStream.Identity.ToString(),
                    Timestamp = eventStream.TimeStamp,
                    eventStream.Version,
                    Data = eventStream.ToString()
                }) != 1)
                {
                    throw new SaeException(StatusCodes.Custom,"EventStream Append Fail");
                }
            }
        }

        public async Task<int> GetVersionAsync(IIdentity identity)
        {
            using (var conn =await this._factory.GetAsync())
            {
                return await conn.ExecuteScalarAsync<int>("select version from EventStream where id=@id order by version desc limit 1", new { id = identity.ToString() });
            }
        }

        public async Task<EventStream> LoadEventStreamAsync(IIdentity identity, int skipEvents, int maxCount)
        {
            var eventStream = new EventStream(identity, 0, events: null, timeStamp: DateTimeOffset.Now);
            using (var conn =await this._factory.GetAsync())
            {
                using (var reader = await conn.ExecuteReaderAsync($"select * from EventStream where id=@id and version > @skipVersion limit {maxCount}",
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
                                           DateTimeOffset.Parse(reader.GetString(reader.GetOrdinal("timestamp")))));
                    }
                }
            }
            return eventStream;
        }

        public async Task RemoveAsync(IIdentity identity)
        {
            using (var conn =await this._factory.GetAsync())
            {
                await conn.ExecuteAsync("delete EventStream where id=@id", new { id = identity.ToString() });
            }
        }
    }
}
