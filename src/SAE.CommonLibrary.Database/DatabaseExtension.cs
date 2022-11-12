using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Database
{
    /// <summary>
    /// <see cref="IDbConnection"/>扩展类
    /// </summary>
    public static class DatabaseExtension
    {
        /// <summary>
        /// 默认名称
        /// </summary>
        private const string DefaultName = "Default";
        /// <summary>
        /// 获得默认数据库链接(异步)
        /// </summary>
        /// <param name="factory"></param>
        public static Task<IDbConnection> GetAsync(this IDBConnectionFactory factory)
        {
            return factory.GetAsync(DefaultName);
        }
        
        /// <summary>
        /// 获得默认数据库链接
        /// </summary>
        /// <param name="factory"></param>
        public static IDbConnection Get(this IDBConnectionFactory factory)
        {
            return factory.Get(DefaultName);
        }
        /// <summary>
        /// 获得与<see cref="DBConnectOptions.Name"/>匹配的数据库链接
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="name">链接名称</param>
        public static IDbConnection Get(this IDBConnectionFactory factory,string name)
        {
            return factory.GetAsync(name).GetAwaiter().GetResult();
        }
    }
}
