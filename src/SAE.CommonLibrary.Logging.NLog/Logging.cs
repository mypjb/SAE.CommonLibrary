using NLog;
using System;
using System.Collections.Generic;

namespace SAE.CommonLibrary.Logging.Nlog
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class Logging : ILogging
    {
        #region Private Readonly Field;

        private readonly ILogger _logging;
        #endregion 

        #region Ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">日志名称</param>
        public Logging(string name = "Default")
        {
            _logging = LogManager.GetLogger(name);
        }


        #endregion Ctor

        #region Log Member
        /// <summary>
        /// 转换记录类型
        /// </summary>
        /// <param name="level">日志类型</param>
        /// <returns>Nlog记录类型</returns>
        private LogLevel Convert(Level level)
        {
            LogLevel logLevel;
            switch (level)
            {
                case Level.Debug: logLevel = LogLevel.Debug; break;
                case Level.Warn: logLevel = LogLevel.Warn; break;
                case Level.Error: logLevel = LogLevel.Error; break;
                case Level.Fatal: logLevel = LogLevel.Fatal; break;
                case Level.Trace: logLevel = LogLevel.Trace; break;
                default: logLevel = LogLevel.Info; break;
            }
            return logLevel;
        }

        /// <inheritdoc/>
        public ILogging Write(string message, Level level, Exception exception)
        {
            LogLevel logLevel = this.Convert(level);
            this._logging.Log(logLevel, exception, message);
            return this;
        }

        /// <inheritdoc/>
        public bool IsEnabled(Level level)
        {
            return this._logging.IsEnabled(this.Convert(level));
        }

        #endregion Log Member
    }
}