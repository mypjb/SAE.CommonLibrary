using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Caching
{
    public interface ICacheIdentityService
    {
        /// <summary>
        /// Calculation <paramref name="o"/> cache key
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        string GetKey(object o);
    }

    public class DefaultCacheIdentityService : ICacheIdentityService
    {
        public string GetKey(object o)
        {
            return o?.ToString() ?? string.Empty;
        }
    }
}
