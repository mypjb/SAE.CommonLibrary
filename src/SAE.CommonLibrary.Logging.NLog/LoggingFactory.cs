using NLog.Config;
using SAE.CommonLibrary.Configuration;

namespace SAE.CommonLibrary.Logging.Nlog
{
    /// <summary>
    /// log工厂
    /// </summary>
    public class LoggingFactory : ILoggingFactory
    {
        public LoggingFactory(IOptionsMonitor<LoggingConfig> monitor)
        {
            var configuration = new XmlLoggingConfiguration(monitor.Option.Document.CreateReader());
            NLog.LogManager.Configuration = configuration;

            monitor.OnChange(option =>
            {
                var configuration = new XmlLoggingConfiguration(monitor.Option.Document.CreateReader());
                NLog.LogManager.Configuration = configuration;
            });
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