using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    /// <summary>
    /// 常量
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// 逻辑操作符
        /// </summary>
        public const string LogicalOperator = @"(\|\|)|(&&)";
        /// <summary>
        /// 关系操作符
        /// </summary>
        public const string RelationalOperator = @"(>=)|(<=)|([><])|(==)|(!=)|(!)";
    }
}