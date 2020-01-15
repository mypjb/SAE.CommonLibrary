﻿using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace SAE.CommonLibrary.Data.MongoDB
{
    public class MongoDBConfig
    {
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

        private int dateType;
        public int DateType
        {
            get => dateType;
            set
            {
                this.dateType = value;
                if (this.dateType == 1)
                {
                    BsonSerializer.RegisterSerializer(typeof(DateTime), DateTimeSerializer.UtcInstance);
                }
                else
                {
                    BsonSerializer.RegisterSerializer(typeof(DateTime), DateTimeSerializer.LocalInstance);
                }
            }
        }
    }


}
