using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.EventStore
{
    /// <summary>
    /// <see cref="IIdentity"/>生成器
    /// </summary>
    public class IdentityGenerator
    {
        /// <summary>
        /// 构建一个拥有默认值得<see cref="IIdentity"/>对象
        /// </summary>
        /// <returns>标识</returns>
        public static IIdentity Build() => new Identity();

        /// <summary>
        /// 根据<paramref name="id"/>构建一个<see cref="IIdentity"/>对象
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns>标识</returns>
        public static IIdentity Build(string id) => new Identity(id);
    }
}
