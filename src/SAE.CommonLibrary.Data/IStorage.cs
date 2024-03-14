using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Data
{
    /// <summary>
    /// 高速存储
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// 返回统一查询接口进行查询
        /// </summary>
        /// <typeparam name="T">查询类型</typeparam>
        /// <returns><see cref="IQueryable{T}"/></returns>
        IQueryable<T> AsQueryable<T>() where T:class;
        
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T">添加的类型</typeparam>
        /// <param name="model">要添加的对象</param>
        Task SaveAsync<T>(T model) where T : class;
        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T">移除的类型</typeparam>
        /// <param name="model">移除的对象</param>
        Task DeleteAsync<T>(T model) where T : class;
        /// <summary>
        /// 根据Id移除对象
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns></returns>
        Task DeleteAsync<T>(object id) where T : class;

    }

}
