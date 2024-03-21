using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Scope
{
    /// <summary>
    /// 区域工厂扩展
    /// </summary>
    public static class ScopeFactoryExtensions
    {
        /// <summary>
        /// <para><em>同步函数</em></para>
        /// <para>获得当前<see cref="IScope"/>对象</para>
        /// </summary>
        /// <param name="factory">区域工厂</param>
        /// <returns>区域</returns>
        public static IScope Get(this IScopeFactory factory)
        {
            return factory.GetAsync().GetAwaiter().GetResult();
        }
        /// <summary>
        /// <para><em>同步函数</em></para>
        /// <para>设置并返回<see cref="IScope"/>对象</para>
        /// </summary>
        /// <param name="factory">区域工厂</param>
        /// <param name="name">区域名称</param>
        /// <returns>区域</returns>
        public static IScope Get(this IScopeFactory factory, string name)
        {
            return factory.GetAsync(name).GetAwaiter().GetResult();
        }
    }
}