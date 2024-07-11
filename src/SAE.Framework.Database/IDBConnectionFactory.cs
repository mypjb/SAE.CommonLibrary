using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.Database
{
    /// <summary>
    /// 数据库连接工厂
    /// </summary>
    public interface IDBConnectionFactory
    {
        /// <summary>
        /// 获得<paramref name="name"/>指定的<see cref="IDbConnection"/>对象
        /// </summary>
        /// <param name="name">数据库链接名称</param>
        Task<IDbConnection> GetAsync(string name);
    }
}
