using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary
{
    /// <summary>
    /// 断言对象
    /// </summary>
    /// <typeparam name="TAssert"></typeparam>
    public interface IAssert<TAssert>
    {
        /// <summary>
        /// 要验证的参数名
        /// </summary>
        string Name { get;  }
        /// <summary>
        /// 当前参数对应的对象
        /// </summary>
        TAssert Current { get; }
    }
}
