using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Logging
{
    /// <summary>
    /// �յ���־��¼����
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