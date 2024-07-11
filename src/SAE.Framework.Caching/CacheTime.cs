using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.Caching
{
    /// <summary>
    /// 缓存时长
    /// </summary>
    public class CacheTime
    {
        /// <summary>
        /// 30 秒
        /// </summary>
        public const int ThirtySeconds = 30;
        /// <summary>
        /// 1 分钟
        /// </summary>
        public const int OneMinute = ThirtySeconds * 2;
        /// <summary>
        /// 5 分钟
        /// </summary>
        public const int FiveMinutes = OneMinute * 5;
        /// <summary>
        /// 10 分钟
        /// </summary>
        public const int TenMinutes = FiveMinutes * 2;
        /// <summary>
        /// 半小时（15分钟）
        /// </summary>
        public const int HalfHour = FiveMinutes * 3;
        /// <summary>
        /// 1 小时
        /// </summary>
        public const int OneHour = HalfHour * 2;
        /// <summary>
        /// 1 天
        /// </summary>
        public const int OneDay = OneHour * 24;
        /// <summary>
        /// 半个月（15天）
        /// </summary>
        public const int HalfMonth = OneDay * 15;
        /// <summary>
        /// 1个月
        /// </summary>
        public const int OneMonth = HalfMonth * 2;
        /// <summary>
        /// 一年
        /// </summary>
        public const int OneYear = OneMonth * 12;
    }
}
