using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.Logging
{
    /// <summary>
    /// 微软日志记录器代理提供者
    /// </summary>
    internal class MicrosoftProxyLoggingProvider : ILoggerProvider
    {
        private readonly ILoggingFactory _loggingFactory;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="loggingFactory">日志工厂</param>
        public MicrosoftProxyLoggingProvider(ILoggingFactory loggingFactory)
        {
            this._loggingFactory = loggingFactory;
        }
        /// <inheritdoc/>
        public ILogger CreateLogger(string categoryName)
        {
           var logging=  this._loggingFactory.Create(categoryName);
            return new MicrosoftProxyLogging(logging);
        }
        /// <inheritdoc/>
        public void Dispose()
        {
        }
    }
}
