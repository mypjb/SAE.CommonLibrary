using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Configuration;
using SAE.CommonLibrary.Configuration.Microsoft;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using StackExchange.Redis;

namespace SAE.CommonLibrary.Caching.Redis
{

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class RedisDistributedCache : IDistributedCache
    {
        private readonly IOptionsMonitor<RedisOptions> _optionsMonitor;
        private readonly ILogging _logging;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsMonitor"></param>
        /// <param name="logging"></param>
        public RedisDistributedCache(IOptionsMonitor<RedisOptions> optionsMonitor, ILogging<RedisDistributedCache> logging)
        {
            this._optionsMonitor = optionsMonitor;
            this._logging = logging;
        }

        private async Task DatabaseOperation(Func<IDatabase, Task> databaseOperation, [CallerMemberName] string methodNmae = null)
        {
            var options = this._optionsMonitor.CurrentValue;

            var database = options.GetDatabase();

            if (database.Multiplexer.IsConnected)
            {
                await databaseOperation(database);
            }
            else
            {
                this._logging.Error($"{methodNmae} => redis connection fail:{database.Database}");
            }
        }
        public async Task<bool> AddAsync<T>(CacheDescription<T> description)
        {
            bool result = false;
            await this.DatabaseOperation(async db =>
            {
                var value = description.Value.ToJsonString();
                result = await db.StringSetAsync(description.Key, value);
                this._logging.Debug($"add cache '{description.Key}':{value}");
            });
            return result;
        }

        public async Task<IEnumerable<bool>> AddAsync<T>(IEnumerable<CacheDescription<T>> descriptions)
        {
            var result = new List<bool>();
            await this.DatabaseOperation(async db =>
            {
                this._logging.Debug($"batch add cache:{descriptions.Count()}");

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
                this._logging.Info($"clear ‘{db.Database}’ db");
                foreach (var endPoint in db.Multiplexer.GetEndPoints())
                {
                    await db.Multiplexer.GetServer(endPoint)
                                        .FlushDatabaseAsync(db.Database);
                    this._logging.Info($"{endPoint} => {db.Database} flush");
                }
            });
            return true;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            string result = null;
            await this.DatabaseOperation(async db =>
            {
                this._logging.Debug($"read:{db.Database} => {key}");
                result = await db.StringGetAsync(key);
            });
            return result.IsNullOrWhiteSpace() ? default(T) : result.ToObject<T>();
        }

        public async Task<IEnumerable<T>> GetAsync<T>(IEnumerable<string> keys)
        {
            IEnumerable<string> results = Enumerable.Empty<string>();
            await this.DatabaseOperation(async db =>
            {
                this._logging.Debug($"batch read:{db.Database}");
                results = (await db.StringGetAsync(keys.Select(s => (RedisKey)s).ToArray()))
                                   .Select(v => (string)v)
                                   .ToArray();
            });
            return results.Select(s =>
            {
                return s.IsNullOrWhiteSpace() ? default(T) : s.ToObject<T>();
            }).ToArray();
        }

        public async Task<IEnumerable<string>> GetKeysAsync()
        {
            List<string> keys = new List<string>();
            await this.DatabaseOperation(db =>
            {
                this._logging.Debug("get all keys ");
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

        public async Task<bool> DeleteAsync(string key)
        {
            var result = false;
            await this.DatabaseOperation(async db =>
            {
                this._logging.Debug($"delete cache: {key}");
                result = await db.KeyDeleteAsync(key);
            });
            return result;
        }

        public async Task<IEnumerable<bool>> DeleteAsync(IEnumerable<string> keys)
        {
            var result = keys.Select(s => false).ToArray();
            await this.DatabaseOperation(async db =>
            {
                this._logging.Debug($"batch delete keys");
                await db.KeyDeleteAsync(keys.Select(key => (RedisKey)key).ToArray());
                result = keys.Select(s => true).ToArray();
            });
            return result;
        }
    }
}