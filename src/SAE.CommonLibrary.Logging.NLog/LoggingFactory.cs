﻿using NLog.Config;
using SAE.CommonLibrary.Configuration;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Logging.Nlog
{
    /// <summary>
    /// log工厂
    /// </summary>
    public class LoggingFactory : ILoggingFactory
    {
        private readonly IOptionsMonitor<LoggingConfig> _monitor;

        public LoggingFactory(IOptionsMonitor<LoggingConfig> monitor)
        {
            this.Config(monitor.Options);
            this._monitor = monitor;
            this._monitor.OnChange(this.Config);
        } 

        private Task Config(LoggingConfig loggingConfig)
        {
            if (loggingConfig.Document != null)
            {
                var configuration = new XmlLoggingConfiguration(loggingConfig.Document.CreateReader());
                NLog.LogManager.Configuration = configuration;
            }
            return Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logName"></param>
        /// <returns></returns>
        public ILogging Create(string logName)
        {
            return new Logging(logName);
        }
        
    }
}