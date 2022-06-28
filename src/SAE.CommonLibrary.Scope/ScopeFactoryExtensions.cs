using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Scope
{
    /// <summary>
    /// scope extension
    /// </summary>
    public static class ScopeFactoryExtensions
    {
        /// <summary>
        /// <para><em>sync method</em></para>
        /// <para>get current <see cref="IScope"/></para>
        /// </summary>
        /// <param name="factory"></param>
        public static IScope Get(this IScopeFactory factory)
        {
            return factory.GetAsync().GetAwaiter().GetResult();
        }
        /// <summary>
        /// <para><em>sync method</em></para>
        /// <para>setting and return current <see cref="IScope"/></para>
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="name">scope identity</param>
        public static IScope Get(this IScopeFactory factory, string name)
        {
            return factory.GetAsync(name).GetAwaiter().GetResult();
        }
    }
}