using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.Logging
{
    /// <summary>
    /// 日志适配器
    /// </summary>
    /// <typeparam name="TCategoryName"></typeparam>
    public class Logging<TCategoryName> : ILogging<TCategoryName>
    {
        private readonly ILogging _logging;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="factory">日志记录器工厂</param>
        public Logging(ILoggingFactory factory)
        {
            _logging = factory.Create<TCategoryName>();
        }
       /// <inheritdoc/>
        public bool IsEnabled(Level level)
        {
            return this._logging.IsEnabled(level);
        }
        /// <inheritdoc/>
        public ILogging Write(string message, Level level, Exception exception)
        {
           return this._logging.Write(message, level, exception);
        }
    }
}
