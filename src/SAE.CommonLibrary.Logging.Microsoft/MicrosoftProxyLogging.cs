using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAE.CommonLibrary.Logging
{
    internal class MicrosoftProxyLogging : ILogger
    {
        private readonly ILogging _proxy;

        public MicrosoftProxyLogging(ILogging proxy)
        {
            _proxy = proxy;
        }


        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!this.IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter.Invoke(state, exception);
            Level level;
            switch (logLevel)
            {
                case LogLevel.Information:
                    {
                        level = Level.Info;
                        break;
                    }
                case LogLevel.Error:
                    {
                        level = Level.Error;
                        break;
                    }
                case LogLevel.Warning:
                    {
                        level = Level.Warn;
                        break;
                    }
                case LogLevel.Debug:
                    {
                        level = Level.Debug;
                        break;
                    }
                case LogLevel.Trace:
                    {
                        level = Level.Info;
                        break;
                    }
                default:
                    {
                        level = Level.Fatal;
                        break;
                    }
            }

            this._proxy.Write(message, level, exception);
        }
    }
}
