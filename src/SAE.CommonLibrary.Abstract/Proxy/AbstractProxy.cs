using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Abstract.Proxy
{
    /// <summary>
    /// 代理抽象实现
    /// </summary>
    /// <typeparam name="TProxy"></typeparam>
    public class AbstractProxy<TProxy>where TProxy:class
    {
        /// <summary>
        /// 代理
        /// </summary>
        protected readonly TProxy _proxy;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="proxy">代理</param>
        public AbstractProxy(TProxy proxy)
        {
            this._proxy = proxy;
        }
    }
}
