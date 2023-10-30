using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 逻辑运算符
    /// </summary>
    public enum LogicalOperator
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        /// <summary>
        /// 逻辑与
        /// </summary>
        And,
        /// <summary>
        /// 逻辑或
        /// </summary>
        Or
    }
}