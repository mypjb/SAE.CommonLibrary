using Microsoft.Extensions.Options;
using NLog.Config;
using SAE.CommonLibrary.Configuration;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Logging.Nlog
{
    /// <summary>
    /// log工厂
    /// </summary>
    public class LoggingFactory : ILoggingFactory
    {
        private readonly IOptionsMonitor<LoggingOptions> _monitor;

        public LoggingFactory(IOptionsMonitor<LoggingOptions> monitor)
        {
            this.Config(monitor.CurrentValue);
            this._monitor = monitor;
            this._monitor.OnChange(this.Config);
        } 

        private void Config(LoggingOptions options)
        {
            if (options.Document != null)
            {
                var configuration = new XmlLoggingConfiguration(options.Document.CreateReader());
                NLog.LogManager.Configuration = configuration;
            }
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