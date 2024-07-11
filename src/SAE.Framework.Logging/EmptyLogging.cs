using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.Framework.Logging
{
    /// <summary>
    /// 空的日志记录器,使用日志对控制台进行输出。
    /// </summary>
    public class EmptyLogging : ILogging
    {
        /// <inheritdoc/>
        public bool IsEnabled(Level level)
        {
            return true;
        }
        /// <inheritdoc/>
        public ILogging Write(string message, Level level, Exception exception)
        {
            Console.WriteLine(message);
            return this;
        }
    }
}