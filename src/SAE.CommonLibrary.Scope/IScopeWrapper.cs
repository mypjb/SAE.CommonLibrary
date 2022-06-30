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
    public interface IScopeWrapper<TService> where TService : class
    {
        /// <summary>
        /// get scope
        /// </summary>
        TService GetService();
    }
}