using System;

namespace SAE.CommonLibrary.Logging.Nlog
{
    /// <summary>
    /// 日志事件
    /// </summary>
    internal class LoggingEvent
    {
        public LoggingEvent()
        {

        }
        public LoggingEvent(string type, string level, string message)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("事件类型尚未提供", nameof(type));
            if (string.IsNullOrWhiteSpace(level))
                throw new ArgumentException("事件级别尚未提供", nameof(level));
            this.Type = type;
            this.Level = level;
            this.Message = message ?? throw new ArgumentException("事件消息体尚未提供", nameof(message));
            this.LocalDate = DateTime.Now;
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 本地时间
        /// </summary>
        public DateTime LocalDate { get; set; }
        /// <summary>
        /// 层
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

    }
}
