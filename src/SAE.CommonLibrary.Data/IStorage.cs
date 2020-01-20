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
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> AsQueryable<T>() where T:class;
        
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        Task SaveAsync<T>(T model) where T : class;
        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        Task RemoveAsync<T>(T model) where T : class;
        /// <summary>
        /// 根据Id移除对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveAsync<T>(object id) where T : class;

    }

}
