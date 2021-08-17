using System;
using System.Collections.Generic;
using System.Text;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.Caching
{
    public enum CacheLimit
    {
        /// <summary>
        /// 无
        /// </summary>
        Nothing,
        /// <summary>
        /// 绝对
        /// </summary>
        Absolute,
        /// <summary>
        /// 滑动
        /// </summary>
        Sliding
    }
    public class CacheDescription
    {
        public CacheDescription(string key, object value)
        {
            Assert.Build(key).NotNullOrWhiteSpace("请提供有效的 'key'");
            Assert.Build(value).NotNull("请提供有效的 'value'");
            this.Key = key;
            this.Value = value;
        }
        /// <summary>
        /// 缓存键
        /// </summary>
        public string Key { get; }
        /// <summary>
        /// 缓存值
        /// </summary>
        public object Value { get; set; }

        private DateTimeOffset? absoluteExpiration;

        /// <summary>
        /// 绝对过期时间
        /// </summary>
        public DateTimeOffset? AbsoluteExpiration
        {
            get => absoluteExpiration;
            set
            {
                if (value != null)
                {
                    absoluteExpiration = value.Value.UtcDateTime;
                }
                else
                {
                    absoluteExpiration = value;
                }
            }
        }
        /// <summary>
        /// 相对于现在的绝对过期
        /// </summary>
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
        /// <summary>
        /// 滑动过期
        /// </summary>
        public TimeSpan? SlidingExpiration { get; set; }
        /// <summary>
        /// 缓存限制类型
        /// </summary>
        public CacheLimit Limit
        {
            get
            {
                CacheLimit limit = CacheLimit.Nothing;

                if (this.SlidingExpiration.HasValue)
                {
                    limit = CacheLimit.Sliding;
                }
                else if (this.AbsoluteExpiration.HasValue || AbsoluteExpirationRelativeToNow.HasValue)
                {
                    limit = CacheLimit.Absolute;
                }

                return limit;
            }
        }

        public DateTimeOffset? GetDateTimeOffset()
        {
            if (this.Limit == CacheLimit.Absolute)
            {
                return this.AbsoluteExpiration.HasValue ? this.AbsoluteExpiration.Value :
                       DateTime.UtcNow.Add(this.AbsoluteExpirationRelativeToNow.Value);
            }
            else if (this.Limit == CacheLimit.Sliding)
            {
                return DateTime.UtcNow.Add(this.SlidingExpiration.Value);
            }
            return null;
        }

        public TimeSpan? GetTimeSpan()
        {
            if (this.Limit == CacheLimit.Absolute)
            {
                return this.AbsoluteExpiration.HasValue ? (DateTime.UtcNow - this.AbsoluteExpiration.Value.UtcDateTime) :
                       this.AbsoluteExpirationRelativeToNow.Value;
            }
            else if (this.Limit == CacheLimit.Sliding)
            {
                return this.SlidingExpiration;
            }
            else
            {
                return null;
            }

        }
    }
}
