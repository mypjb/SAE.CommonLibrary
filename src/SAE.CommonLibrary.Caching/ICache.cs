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
        Task<bool> AddAsync(CacheDescription description);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="descriptions"></param>
        /// <returns></returns>
        Task<IEnumerable<bool>> AddAsync(IEnumerable<CacheDescription> descriptions);
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<object> GetAsync(string key);
        /// <summary>
        /// 获得多个结果
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<IEnumerable<object>> GetAsync(IEnumerable<string> keys);
        /// <summary>
        /// 根据<paramref name="key"/>移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(string key);
        /// <summary>
        /// 移除所有匹配键
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<IEnumerable<bool>> RemoveAsync(IEnumerable<string> keys);
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
