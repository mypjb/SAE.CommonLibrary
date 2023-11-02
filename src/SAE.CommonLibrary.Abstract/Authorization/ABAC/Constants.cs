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
        /// <summary>
        /// 属性前缀
        /// </summary>
        public const char PropertyPrefix = '$';

        public class Regex
        {
            /// <summary>
            /// float格式
            /// </summary>
            public const string FloatPattern = "^\\d[\\d\\.]*";
            /// <summary>
            /// 日期正则 yyyy-MM-dd hh:mm:ss
            /// </summary>
            /// <value></value>
            public const string DateTimePattern = "\\d{4}-\\d{2}-\\d{2} \\d{2}:\\d{2}:\\d{2}";
            /// <summary>
            /// hh:mm:ss 时间格式
            /// </summary>
            /// <value></value>
            public const string TimeSpanPattern = "\\d{2,}:\\d{2}:\\d{2}";
            /// <summary>
            /// Bool格式
            /// </summary>
            public const string BoolPattern = "$(true)|(false)^";
            /// <summary>
            /// 字符格式
            /// </summary>
            public const string StringPattern = "$\".*\"^";
        }
    }
}