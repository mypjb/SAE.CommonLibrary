using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SAE.CommonLibrary.Logging;
using System;

namespace SAE.CommonLibrary.Data.MongoDB
{
    /// <summary>
    /// mongodb 配置
    /// </summary>
    public class MongoDBOptions
    {
        /// <summary>
        /// 配置节名称
        /// </summary>
        public const string Option = "mongodb";
        /// <summary>
        /// 库
        /// </summary>
        public string DB { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Connection { get; set; }

        private int dateType;
        /// <summary>
        /// 日期数据类型
        /// </summary>
        /// <remarks>
        /// 1：UTC时间，0：本地时间
        /// </remarks>
        public int DateType
        {
            get => dateType;
            set
            {
                this.dateType = value;
                try
                {
                    if (this.dateType == 1)
                    {
                        BsonSerializer.RegisterSerializer(typeof(DateTime), DateTimeSerializer.UtcInstance);
                    }
                    else
                    {
                        BsonSerializer.RegisterSerializer(typeof(DateTime), DateTimeSerializer.LocalInstance);
                    }
                }
                catch (BsonSerializationException ex)
                {
                }

            }
        }

        private IMongoDatabase mongoDatabase;
        /// <summary>
        /// 获得mongodb链接
        /// </summary>
        public IMongoDatabase GetDatabase()
        {
            if (mongoDatabase == null)
            {
                var clientSettings = MongoClientSettings.FromConnectionString(this.Connection);
                clientSettings.LinqProvider = LinqProvider.V3;
                var client = new MongoClient(clientSettings);
                this.mongoDatabase = client.GetDatabase(this.DB);
            }
            return mongoDatabase;
        }
    }


}
