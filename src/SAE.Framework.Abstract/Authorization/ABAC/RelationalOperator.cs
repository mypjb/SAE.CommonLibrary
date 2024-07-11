using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.Framework.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 关系运算符
    /// </summary>
    public enum RelationalOperator
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan,
        /// <summary>
        /// 小于
        /// </summary>
        LessThan,
        /// <summary>
        /// 等于
        /// </summary>
        Equal,
        ///<summary>
        /// 不等于
        ///</summary>
        NotEqual,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessThanOrEqual,
        ///<summary>
        /// 否定
        /// </summary>
        Not,
        /// <summary>
        /// 包含
        /// </summary>
        Include,
        /// <summary>
        /// 正则表达式
        /// </summary>
        Regex
    }
}