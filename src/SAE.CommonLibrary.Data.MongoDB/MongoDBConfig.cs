using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace SAE.CommonLibrary.Data.MongoDB
{
    public class MongoDBConfig
    {
        private readonly static object _lock = new object();
        static MongoDBConfig()
        {
            lock (_lock)
            {
                BsonSerializer.RegisterSerializer(typeof(DateTime), DateTimeSerializer.LocalInstance);
            }
            
        }
        public MongoDBConfig()
        {
            
        }
        /// <summary>
        /// 库
        /// </summary>
        public string DB { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Connection { get; set; }
        
    }

   
}
