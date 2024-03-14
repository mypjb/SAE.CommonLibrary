using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SAE.CommonLibrary.Configuration.Microsoft;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.Data.MongoDB
{
    /// <summary>
    /// <see cref="IStorage"/>Mongo实现
    /// </summary>
    public class MongoDBStorage : IStorage
    {
        private const string IdName = "_id";
        #region Private Member
        private readonly ILogging _logging;
        private readonly IMetadataProvider _descriptionProvider;
        private readonly IOptionsMonitor<MongoDBOptions> _optionsMonitor;
        #endregion

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsMonitor">配置监控器</param>
        /// <param name="logging">日志记录器></param>
        /// <param name="descriptionProvider">元数据提供者</param>
        public MongoDBStorage(IOptionsMonitor<MongoDBOptions> optionsMonitor,
                              ILogging<MongoDBStorage> logging,
                              IMetadataProvider descriptionProvider)
        {
            this._logging = logging;
            this._descriptionProvider = descriptionProvider;
            this._optionsMonitor = optionsMonitor;
        }
        /// <summary>
        /// 获得mongodb里的一个<c>collection</c>.
        /// </summary>
        /// <typeparam name="T">collection类型</typeparam>
        private IMongoCollection<T> GetCollection<T>() where T : class
        {
            var options = this._optionsMonitor.CurrentValue;
            var mongoDatabase = options.GetDatabase();
            var description = this._descriptionProvider.Get<T>();
            this._logging.Debug($"访问数据库：{options.DB}中的{description.Name}表");
            return mongoDatabase.GetCollection<T>(description.Name);
        }
        /// <inheritdoc/>
        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return this.GetCollection<T>().AsQueryable();
        }
        /// <inheritdoc/>
        public async Task DeleteAsync<T>(T model) where T : class
        {
            if (model == null) return;

            var id = this._descriptionProvider.Get<T>().IdentiyFactory.Invoke(model);

            var query = new QueryDocument(IdName, BsonValue.Create(id));

            var collection = this.GetCollection<T>();

            await collection.DeleteOneAsync(query);

            this._logging.Debug($"Remove {collection.CollectionNamespace}:{id}");
        }
        /// <inheritdoc/>
        public async Task DeleteAsync<T>(object id) where T : class
        {
            if (id == null) return;

            var query = new QueryDocument(IdName, BsonValue.Create(id));

            var collection = this.GetCollection<T>();

            await collection.DeleteOneAsync(query);

            this._logging.Debug($"Remove {collection.CollectionNamespace}:{id}");
        }
        /// <inheritdoc/>
        public async Task SaveAsync<T>(T model) where T : class
        {
            if (model == null) return;

            this._logging.Debug("Execute Save");

            var id = this._descriptionProvider.Get<T>().IdentiyFactory.Invoke(model);

            var query = new QueryDocument(IdName, BsonValue.Create(id));

            await this.GetCollection<T>()
                      .ReplaceOneAsync(query, model, new ReplaceOptions { IsUpsert = true });
        }
    }
}
