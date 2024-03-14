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
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configuration">配置</param>
        public LoggingFactory(IConfiguration configuration)
        {
            this.Config(configuration.GetSection(LoggingOptions.Option));
        } 
        /// <summary>
        /// 配置日志
        /// </summary>
        /// <param name="section"></param>
        private void Config(IConfigurationSection section)
        {
            NLog.LogManager.Configuration = new NLogLoggingConfiguration(section);
        }

        /// <inheritdoc/>
        public ILogging Create(string logName)
        {
            return new Logging(logName);
        }
        
    }
}