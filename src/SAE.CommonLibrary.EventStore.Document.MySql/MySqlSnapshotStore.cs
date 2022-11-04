using Dapper;
using SAE.CommonLibrary.Database;
using SAE.CommonLibrary.EventStore.Snapshot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.EventStore.Document.MySql
{
    /// <summary>
    /// 基于内存的快照存储
    /// </summary>
    public class MySqlSnapshotStore : ISnapshotStore
    {
        private readonly IDBConnectionFactory _factory;

        public MySqlSnapshotStore(IDBConnectionFactory factory)
        {
            this._factory = factory;
        }
        public async Task<Snapshot.Snapshot> FindAsync(IIdentity identity, int version)
        {
            Snapshot.Snapshot snapshot;

            using (var conn = this._factory.Get())
            {
                snapshot = await conn.QueryFirstOrDefaultAsync<Snapshot.Snapshot>($"select * from {nameof(Snapshot)} where id=@id and version=@version limit 1", new
                {
                    Id = identity.ToString(),
                    Version = version
                }) ?? new Snapshot.Snapshot();
            }

            snapshot.Id = identity.ToString();

            return snapshot;
        }

        public async Task<Snapshot.Snapshot> FindAsync(IIdentity identity)
        {
            Snapshot.Snapshot snapshot;

            using (var conn = this._factory.Get())
            {
                snapshot = await conn.QueryFirstOrDefaultAsync<Snapshot.Snapshot>($"select * from {nameof(Snapshot)} where id=@id order by version desc limit 1", new
                {
                    Id = identity.ToString()
                }) ?? new Snapshot.Snapshot();
            }
            snapshot.Id = identity.ToString();
            return snapshot;
        }

        public async Task DeleteAsync(IIdentity identity)
        {
            using (var conn = this._factory.Get())
            {
                await conn.ExecuteAsync($"delete from  {nameof(Snapshot)} where id=@id", new { id = identity.ToString() });
            }
        }

        public async Task SaveAsync(Snapshot.Snapshot snapshot)
        {
            using (var conn = this._factory.Get())
            {
                if (await conn.ExecuteAsync($"insert into {nameof(Snapshot)}(id,version,data) values(@id,@version,@data)", snapshot) != 1)
                {
                    throw new SAEException($"{nameof(snapshot)} add fail");
                }
            }
        }
    }
}
