using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NLog.Config;
using NLog.Extensions.Logging;
using SAE.CommonLibrary.Configuration;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Logging.Nlog
{
    /// <summary>
    /// log工厂
    /// </summary>
    public class LoggingFactory : ILoggingFactory
    {
        public LoggingFactory(IConfiguration configuration)
        {
            this.Config(configuration.GetSection(LoggingOptions.Option));
        } 

        private void Config(IConfigurationSection section)
        {
            NLog.LogManager.Configuration = new NLogLoggingConfiguration(section);
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