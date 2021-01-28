using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace SAE.CommonLibrary.Data.MongoDB
{
    public class MongoDBOptions
    {
        public const string Option = "mongodb";
        public MongoDBOptions()
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

        private int dateType;
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
    }


}
