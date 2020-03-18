using SAE.CommonLibrary.Configuration;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Caching.Redis
{
    public class RedisDistributedCache : IDistributedCache
    {
        private readonly ILogging _logging;

        protected IConnectionMultiplexer ConnectionMultiplexer { get; private set; }
        protected IDatabase Database { get; private set; }
        public RedisDistributedCache(IOptionsMonitor<RedisOptions> monitor, ILogging<RedisDistributedCache> logging)
        {
            this._logging = logging;
            this.Configure(monitor.Options);
            monitor.OnChange(this.Configure);
        }

        private Task Configure(RedisOptions options)
        {
            var connectMessage = $"connect:'{options.Connection}',db:'{options.DB}'";
            if (this.ConnectionMultiplexer == null)
            {
                this._logging.Info($"初始化连接 {connectMessage}");
            }
            else
            {
                this._logging.Info($"更改缓存配置 {connectMessage}");
            }


            this.ConnectionMultiplexer = StackExchange.Redis.ConnectionMultiplexer.Connect(options.Connection);

            this.Database = this.ConnectionMultiplexer.GetDatabase(options.DB);

            return Task.CompletedTask;
        }

        private async Task DatabaseOperation(Func<IDatabase, Task> databaseOperation, [CallerMemberName] string methodNmae = null)
        {
            if (this.ConnectionMultiplexer.IsConnected)
            {
                await databaseOperation(this.Database);
            }
            else
            {
                this._logging.Error($"{methodNmae} => 服务器连接失败，请检查网络是否正常");
            }
        }
        public async Task<bool> AddAsync(CacheDescription description)
        {
            bool result = false;
            await this.DatabaseOperation(async db =>
            {
                var value = description.Value.ToJsonString();
                result = await db.StringSetAsync(description.Key, value);
                this._logging.Debug($"添加缓存 '{description.Key}':{value}");
            });
            return result;
        }

        public async Task<IEnumerable<bool>> AddAsync(IEnumerable<CacheDescription> descriptions)
        {
            var result = new List<bool>();
            await this.DatabaseOperation(async db =>
            {
                this._logging.Debug($"批量添加缓存:{descriptions.Count()}");

                var batch = db.CreateBatch();

                IList<Task<bool>> tasks = new List<Task<bool>>();

                foreach (var description in descriptions)
                    tasks.Add(batch.StringSetAsync(description.Key, description.Value.ToJsonString(), description.GetTimeSpan()));
                batch.Execute();
                await tasks.ForEachAsync(async task =>
                {
                    result.Add(await task);
                });
            });
            return result;
        }

        public async Task<bool> ClearAsync()
        {
            await this.DatabaseOperation(async db =>
            {
                this._logging.Info($"对‘{db.Database}’执行清库操作");
                foreach (var endPoint in db.Multiplexer.GetEndPoints())
                {
                    await db.Multiplexer.GetServer(endPoint)
                                        .FlushDatabaseAsync(db.Database);
                    this._logging.Info($"{endPoint} => {db.Database} flush");
                }
            });
            return true;
        }

        public async Task<object> GetAsync(string key)
        {
            string result = null;
            await this.DatabaseOperation(async db =>
            {
                this._logging.Debug($"读取缓存:{db.Database} => {key}");
                result = await db.StringGetAsync(key);
            });
            return result;
        }

        public async Task<IEnumerable<object>> GetAsync(IEnumerable<string> keys)
        {
            IEnumerable<string> results = Enumerable.Empty<string>();
            await this.DatabaseOperation(async db =>
            {
                this._logging.Debug($"批量读取缓存:{db.Database}");
                results = (await db.StringGetAsync(keys.Select(s => (RedisKey)s).ToArray()))
                                   .Select(v => (string)v)
                                   .ToArray();
            });
            return results;
        }

        public async Task<IEnumerable<string>> GetKeysAsync()
        {
            List<string> keys = new List<string>();
            await this.DatabaseOperation(db =>
            {
                this._logging.Debug("获得所有缓存键");
                foreach (var endPoint in db.Multiplexer.GetEndPoints())
                {
                    this._logging.Debug($"{endPoint} => {db.Database} keys");
                    keys.AddRange(db.Multiplexer.GetServer(endPoint)
                                        .Keys(db.Database, pageSize: 10000)
                                        .Select(s => (string)s)
                                        .ToArray());
                }
                return Task.CompletedTask;
            });

            return keys.Distinct();
        }

        public async Task<bool> RemoveAsync(string key)
        {
            var result = false;
            await this.DatabaseOperation(async db =>
            {
                this._logging.Debug($"移除缓存 {key}");
                result = await db.KeyDeleteAsync(key);
            });
            return result;
        }

        public async Task<IEnumerable<bool>> RemoveAsync(IEnumerable<string> keys)
        {
            var result = keys.Select(s => false).ToArray();
            await this.DatabaseOperation(async db =>
            {
                this._logging.Debug($"批量移除缓存");
                await db.KeyDeleteAsync(keys.Select(key => (RedisKey)key).ToArray());
                var result = keys.Select(s => true).ToArray();
            });
            return result;
        }
    }
}
