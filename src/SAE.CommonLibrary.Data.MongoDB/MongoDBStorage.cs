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
    /// <inheritdoc/>
    /// </summary>
    public class MongoDBStorage : IStorage
    {
        #region Private Member
        private readonly Type _stringType = typeof(string);
        private readonly IDictionary<Type, Delegate> _idDelegateStorage = new Dictionary<Type, Delegate>();
        private readonly ILogging _logging;
        private readonly IMetadataProvider _descriptionProvider;
        private readonly IOptionsManage<MongoDBOptions, IMongoDatabase> _optionsManage;
        #endregion

        #region Ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="log"></param>
        public MongoDBStorage(IOptionsManage<MongoDBOptions, IMongoDatabase> optionsManage,
                              ILogging<MongoDBStorage> logging,
                              IMetadataProvider descriptionProvider)
        {
            this._logging = logging;
            this._descriptionProvider = descriptionProvider;
            this._optionsManage = optionsManage;
            this._optionsManage.OnConfigure += this.Configure;
        }
        #endregion
        private IMongoDatabase Configure(MongoDBOptions options)
        {
            this._logging.Debug($"Connection={options.Connection},DB={options.DB}");
            var client = new MongoClient(new MongoUrl(options.Connection));
            return client.GetDatabase(options.DB);
        }

        private IMongoCollection<T> GetCollection<T>() where T : class
        {
            var mongoDatabase = this._optionsManage.Get();
            var description = this._descriptionProvider.Get<T>();
            return mongoDatabase.GetCollection<T>(description.Name);
        }

        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return this.GetCollection<T>().AsQueryable();
        }

        public async Task DeleteAsync<T>(T model) where T : class
        {
            if (model == null) return;

            var id = this._descriptionProvider.Get<T>().IdentiyFactory.Invoke(model);

            var query = new QueryDocument("_id", BsonValue.Create(id));

            var collection = this.GetCollection<T>();

            await collection.DeleteOneAsync(query);

            this._logging.Debug($"Remove {collection.CollectionNamespace}:{id}");
        }

        public async Task DeleteAsync<T>(object id) where T : class
        {
            if (id == null) return;

            var query = new QueryDocument("_id", BsonValue.Create(id));

            var collection = this.GetCollection<T>();

            await collection.DeleteOneAsync(query);

            this._logging.Debug($"Remove {collection.CollectionNamespace}:{id}");
        }

        public async Task SaveAsync<T>(T model) where T : class
        {
            if (model == null) return;

            this._logging.Debug("Execute Save");

            var id = this._descriptionProvider.Get<T>().IdentiyFactory.Invoke(model);

            var query = new QueryDocument("_id", BsonValue.Create(id));

            await this.GetCollection<T>()
                      .ReplaceOneAsync(query, model, new ReplaceOptions { IsUpsert = true });
        }
    }
}
