using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Caching
{
    public interface ICache
    {
        /// <summary>
        /// 根据<paramref name="description"/>添加缓存
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<bool> AddAsync<T>(CacheDescription<T> description);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="descriptions"></param>
        /// <returns></returns>
        Task<IEnumerable<bool>> AddAsync<T>(IEnumerable<CacheDescription<T>> descriptions);
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);
        /// <summary>
        /// 获得多个结果
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAsync<T>(IEnumerable<string> keys);
        /// <summary>
        /// 根据<paramref name="key"/>移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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
        /// <returns></returns>
        Task<bool> ClearAsync();
        /// <summary>
        /// 获得所有键
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<string>> GetKeysAsync();
    }
}
