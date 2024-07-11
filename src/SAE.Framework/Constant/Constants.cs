using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SAE.Framework
{
    
    /// <summary>
    /// 系统常量
    /// </summary>
    public partial class Constants
    {
        /// <summary>
        /// 默认编码
        /// </summary>
        public static Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// 时区
        /// </summary>
        public static Func<DateTime> TimeZoneGenerator = () => DateTime.UtcNow;

        /// <summary>
        /// 默认名称
        /// </summary>
        public const string Default = "";
        /// <summary>
        /// 默认区域配置
        /// </summary>
        public const string Scope = nameof(Scope);
        /// <summary>
        /// 默认时区
        /// </summary>
        public static DateTime DefaultTimeZone
        {
            get => TimeZoneGenerator.Invoke();
        }
    }
}
