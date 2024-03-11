using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Caching
{
    /// <summary>
    /// 缓存标识服务
    /// </summary>
    public interface ICacheIdentityService
    {
        /// <summary>
        /// 计算<paramref name="o"/>生成缓存Key
        /// </summary>
        /// <param name="o">参与计算的对象</param>
        /// <returns>缓存键</returns>
        string GetKey(object o);
    }
    /// <summary>
    /// <see cref="ICacheIdentityService"/>默认实现
    /// </summary>
    public class DefaultCacheIdentityService : ICacheIdentityService
    {
        /// <inheritdoc/>
        public string GetKey(object o)
        {
            return o?.ToString() ?? string.Empty;
        }
    }
}
