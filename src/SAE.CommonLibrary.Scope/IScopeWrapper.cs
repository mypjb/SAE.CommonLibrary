using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Scope
{
    /// <summary>
    /// scope service wrapper
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    [Obsolete("this interface is in preview stage")]
    public interface IScopeWrapper<TService> where TService : class
    {
        /// <summary>
        /// get scope
        /// <param name="key"> cache key</param>
        /// <param name="constructor"><typeparamref name="TService"/> constructor</param>>
        /// </summary>
        TService GetService(string key, Func<TService> constructor);

        /// <summary>
        /// clear
        /// </summary>
        void Clear();
    }
}