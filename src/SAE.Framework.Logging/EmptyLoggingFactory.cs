using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.Framework.Logging
{
    /// <summary>
    /// 空的日志工厂实现
    /// </summary>
    public class EmptyLoggingFactory : ILoggingFactory
    {
        /// <inheritdoc/>
        public ILogging Create(string logName)
        {
            return new EmptyLogging();
        }
    }
}