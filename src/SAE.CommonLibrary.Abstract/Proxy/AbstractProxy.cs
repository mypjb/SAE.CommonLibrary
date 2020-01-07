using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Abstract.Proxy
{
    public class AbstractProxy<TProxy>where TProxy:class
    {
        protected readonly TProxy _proxy;

        public AbstractProxy(TProxy proxy)
        {
            this._proxy = proxy;
        }
    }
}
