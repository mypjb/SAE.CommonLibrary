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

        private readonly ILogger _log;
        #endregion 

        #region Ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public Logging(string name = "Default")
        {
            _log = LogManager.GetLogger(name);
        }
       

        #endregion Ctor

        #region Log Member

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

        ///<summary>
        ///写日志
        ///</summary>
        ///<param name="message">日志消息</param>
        ///<param name="level">日志类型</param>
        ///<param name="exception">异常</param>
        public ILogging Write(string message, Level level, Exception exception)
        {
            LogLevel logLevel = this.Convert(level);
            this._log.Log(logLevel, exception, message);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled(Level level)
        {
            return this._log.IsEnabled(this.Convert(level));
        }

        #endregion Log Member
    }
}