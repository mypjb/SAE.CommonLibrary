using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Logging
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class EmptyLoggingFactory : ILoggingFactory
    {
        public ILogging Create(string logName)
        {
            return new EmptyLogging();
        }
    }
}