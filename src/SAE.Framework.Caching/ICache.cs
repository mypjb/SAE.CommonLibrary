using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.Caching
{
    /// <summary>
    /// 缓存基础接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 根据<paramref name="description"/>添加缓存
        /// </summary>
        /// <param name="description">缓存描述</param>
        /// <returns>true:添加成功,false:添加失败</returns>
        Task<bool> AddAsync<T>(CacheDescription<T> description);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="descriptions">缓存描述集</param>
        /// <returns>true:添加成功,false:添加失败</returns>
        Task<IEnumerable<bool>> AddAsync<T>(IEnumerable<CacheDescription<T>> descriptions);
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存
        /// </summary>
        /// <param name="key">获得键</param>
        /// <returns>缓存值</returns>
        Task<T> GetAsync<T>(string key);
        /// <summary>
        /// 获得多个结果
        /// </summary>
        /// <param name="keys">获得键集合</param>
        /// <returns>缓存值</returns>
        Task<IEnumerable<T>> GetAsync<T>(IEnumerable<string> keys);
        /// <summary>
        /// 根据<paramref name="key"/>移除缓存
        /// </summary>
        /// <param name="key">获得键</param>
        /// <returns>true:删除成功,false:删除失败</returns>
        Task<bool> DeleteAsync(string key);
        /// <summary>
        /// 移除所有匹配键
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<IEnumerable<bool>> DeleteAsync(IEnumerable<string> keys);
        /// <summary>
        /// 移除所有缓存
        /// </summary>
        /// <returns>true:清理成功,false:清理失败</returns>
        Task<bool> ClearAsync();
        /// <summary>
        /// 获得所有键
        /// </summary>
        /// <returns>缓存键集合</returns>
        Task<IEnumerable<string>> GetKeysAsync();
        /// <summary>
        /// 缓存是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>true:缓存存在,false:缓存失败</returns>
        Task<bool> ExistAsync(string key);
    }
}
