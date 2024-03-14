using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Scope
{
    /// <summary>
    /// 区域服务包装器
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    [Obsolete("this interface is in preview stage")]
    public interface IScopeWrapper<TService> where TService : class
    {
        /// <summary>
        /// 获得当前区域
        /// <param name="key">缓存key</param>
        /// <param name="constructor"><typeparamref name="TService"/>构造服务</param>>
        /// </summary>
        TService GetService(string key, Func<TService> constructor);

        /// <summary>
        /// 清理
        /// </summary>
        void Clear();
    }
}