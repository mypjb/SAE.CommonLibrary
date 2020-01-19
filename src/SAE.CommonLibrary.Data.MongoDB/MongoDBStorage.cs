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
        private readonly IMongoDatabase _database;
        #endregion

        #region Ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="log"></param>
        public MongoDBStorage(MongoDBConfig config,
                              ILogging<MongoDBStorage> logging)
        {
            this._logging = logging;
            this._logging.Debug($"Connection={config.Connection},DB={config.DB}");
            var client = new MongoClient(new MongoUrl(config.Connection));
            this._database = client.GetDatabase(config.DB);
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        public async Task AddAsync<T>(T model)
        {
            if (model == null) return;
            this._logging.Debug("Execute Add");
            await this.GetCollection<T>().InsertOneAsync(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        public async Task UpdateAsync<T>(T model)
        {
            if (model == null) return;

            this._logging.Debug("Execute Update");

            var id = IdentityDelegate(model);

            var query = new QueryDocument("_id", BsonValue.Create(id));

            await this.GetCollection<T>()
                      .ReplaceOneAsync(query, model, new ReplaceOptions { IsUpsert = true });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> AsQueryable<T>()
        {
            return this.GetCollection<T>().AsQueryable();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        public async Task RemoveAsync<T>(T model)
        {
            if (model == null) return;

            var id = IdentityDelegate(model);

            var query = new QueryDocument("_id", BsonValue.Create(id));
            
            var collection = this.GetCollection<T>();

            await collection.DeleteOneAsync(query);

            this._logging.Info($"Remove {collection.CollectionNamespace}:{id}");
        }

        /// <summary>
        /// 标识表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        private object IdentityDelegate<T>(T model)
        {
            var type = typeof(T);
            Delegate @delegate;
            if (!_idDelegateStorage.TryGetValue(type, out @delegate))
            {
                _logging.Info("Identity Delegate:Get Id Property");
                var property = type.GetTypeInfo()
                                   .GetProperties()
                                   .Where(s => s.Name.ToLower() == "_id" || s.Name.ToLower() == "id")
                                   .FirstOrDefault();

                if (property == null)
                {
                    _logging.Error("MongoDB Document Class You have to have a primary key");
                    throw new ArgumentNullException(nameof(model), $"{nameof(model)}中必须要有一个，唯一的键。默认为\"_id或\"\"id\"");
                }
                if (property.PropertyType.GetTypeInfo().IsValueType || property.PropertyType == _stringType)
                {
                    var p = Expression.Parameter(typeof(T));
                    var body = Expression.Property(p, property.Name);
                    var expression = Expression.Lambda(body, p);
                    @delegate = expression.Compile();
                    _idDelegateStorage[type] = @delegate;
                }
            }

            return @delegate.DynamicInvoke(model);
        }

        private IMongoCollection<T> GetCollection<T>()
        {
            return this._database.GetCollection<T>(typeof(T).Name.ToLower());
        }
    }
}
