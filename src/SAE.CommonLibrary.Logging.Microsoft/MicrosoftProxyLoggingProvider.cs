using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Logging
{
    internal class MicrosoftProxyLoggingProvider : ILoggerProvider
    {
        private readonly ILoggingFactory _loggingFactory;

        public MicrosoftProxyLoggingProvider(ILoggingFactory loggingFactory)
        {
            this._loggingFactory = loggingFactory;
        }
        public ILogger CreateLogger(string categoryName)
        {
           var logging=  this._loggingFactory.Create(categoryName);
            return new MicrosoftProxyLogging(logging);
        }

        public void Dispose()
        {
        }
    }
}
