using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary
{
    public interface IAssert<TAssert>
    {
        /// <summary>
        /// 要验证的参数名
        /// </summary>
        string Name { get;  }
        TAssert Current { get; }
    }
}
