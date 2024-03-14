using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Logging
{
    /// <summary>
    /// �յ���־��¼��
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