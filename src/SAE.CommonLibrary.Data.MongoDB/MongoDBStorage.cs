using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Data.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class MongoDBStorage : IStorage
    {
        #region Private Member
        private readonly Type _stringType = typeof(string);
        private readonly IDictionary<Type, Delegate> _idDelegateStorage = new Dictionary<Type, Delegate>();
        private readonly ILogging _logging;
        private readonly IMetadataProvider _descriptionProvider;
        private readonly IMongoDatabase _database;
        #endregion

        #region Ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="log"></param>
        public MongoDBStorage(MongoDBConfig config,
                              ILogging<MongoDBStorage> logging,
                              IMetadataProvider descriptionProvider)
        {
            this._logging = logging;
            this._descriptionProvider = descriptionProvider;
            this._logging.Debug($"Connection={config.Connection},DB={config.DB}");
            var client = new MongoClient(new MongoUrl(config.Connection));
            this._database = client.GetDatabase(config.DB);
        }
        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return this.GetCollection<T>().AsQueryable();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        public async Task RemoveAsync<T>(T model) where T : class
        {
            if (model == null) return;

            var id = this._descriptionProvider.Get<T>().IdentiyFactory.Invoke(model);

            var query = new QueryDocument("_id", BsonValue.Create(id));
            
            var collection = this.GetCollection<T>();

            await collection.DeleteOneAsync(query);

            this._logging.Debug($"Remove {collection.CollectionNamespace}:{id}");
        }

        private IMongoCollection<T> GetCollection<T>() where T : class
        {
            var description= this._descriptionProvider.Get<T>();
            return this._database.GetCollection<T>(description.Name);
        }

        public async Task RemoveAsync<T>(object id) where T : class
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

        #region Add And Update
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="model"></param>
        //public async Task AddAsync<T>(T model) where T : class
        //{
        //    if (model == null) return;
        //    this._logging.Debug("Execute Add");
        //    await this.GetCollection<T>().InsertOneAsync(model);
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="model"></param>
        //public async Task UpdateAsync<T>(T model) where T : class
        //{
        //    if (model == null) return;

        //    this._logging.Debug("Execute Update");

        //    var id = this._descriptionProvider.Get<T>().IdentiyFactory.Invoke(model);

        //    var query = new QueryDocument("_id", BsonValue.Create(id));

        //    await this.GetCollection<T>()
        //              .ReplaceOneAsync(query, model, new ReplaceOptions { IsUpsert = true });
        //}
        #endregion
    }
}
