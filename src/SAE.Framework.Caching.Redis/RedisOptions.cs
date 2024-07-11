using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace SAE.Framework.Caching.Redis
{
    /// <summary>
    /// redis配置项
    /// </summary>
    public class RedisOptions : IDisposable
    {
        /// <summary>
        /// 析构函数
        /// </summary>
        ~RedisOptions()
        {
            this.Dispose();
        }
        /// <summary>
        /// redis 数据库接口
        /// </summary>
        private IDatabase _database;
        /// <summary>
        /// 配置节名称
        /// </summary>
        public const string Option = "redis";
        /// <summary>
        /// 链接字符串
        /// </summary>
        /// <value></value>
        public string Connection { get; set; }
        /// <summary>
        /// DB number
        /// </summary>
        /// <value></value>
        public int DB { get; set; }
        /// <summary>
        /// 获得redis链接
        /// </summary>
        public IDatabase GetDatabase()
        {
            if (this._database == null)
            {
                var connectionMultiplexer = ConnectionMultiplexer.Connect(this.Connection);
                this._database = connectionMultiplexer.GetDatabase(this.DB);
            }

            return this._database;
        }
        /// <summary>
        /// 销毁实例化后的链接
        /// </summary>
        public void Dispose()
        {
            if (this._database != null)
                this._database.Multiplexer.Dispose();
        }
    }
}
