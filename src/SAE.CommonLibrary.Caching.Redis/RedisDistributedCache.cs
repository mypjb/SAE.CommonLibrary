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
        private readonly IOptionsManage<RedisOptions, Tuple<IDatabase, IConnectionMultiplexer>> _optionsManage;
        private readonly ILogging _logging;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsManage"></param>
        /// <param name="logging"></param>
        public RedisDistributedCache(IOptionsManage<RedisOptions, Tuple<IDatabase, IConnectionMultiplexer>> optionsManage, ILogging<RedisDistributedCache> logging)
        {
            this._optionsManage = optionsManage;
            this._logging = logging;
            this._optionsManage.OnChange+=this.DisplayConnection;
            this._optionsManage.OnConfigure+=this.Configure;
        }

        private void DisplayConnection(RedisOptions options, Tuple<IDatabase, IConnectionMultiplexer> tuple)
        {
            var connect=tuple.Item2;
            try
                {
                    var message = $"dispose connection {connect.Configuration}";
                    this._logging.Info($"begin {message}");
                    connect.Dispose();
                    this._logging.Info($"end {message}");
                }
                catch (Exception ex)
                {
                    this._logging.Error(ex, "exception occurred while cleaning up old links");
                }
        }

        private Tuple<IDatabase, IConnectionMultiplexer> Configure(RedisOptions options)
        {
            var connectMessage = $"connect:'{options.Connection}',db:'{options.DB}'";

            this._logging.Info($"initial {connectMessage}");

            var connectionMultiplexer = ConnectionMultiplexer.Connect(options.Connection);

            return Tuple.Create<IDatabase, IConnectionMultiplexer>(connectionMultiplexer.GetDatabase(options.DB), connectionMultiplexer);

        }

        private async Task DatabaseOperation(Func<IDatabase, Task> databaseOperation, [CallerMemberName] string methodNmae = null)
        {
            var tuple = this._optionsManage.Get();

            if (tuple.Item2.IsConnected)
            {
                await databaseOperation(tuple.Item1);
            }
            else
            {
                this._logging.Error($"{methodNmae} => redis connection fail");
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
                var result = keys.Select(s => true).ToArray();
            });
            return result;
        }
    }
}