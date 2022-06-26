using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Logging
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class EmptyLogging : ILogging
    {
        public bool IsEnabled(Level level)
        {
            return true;
        }

        public ILogging Write(string message, Level level, Exception exception)
        {
            Console.WriteLine(message);
            return this;
        }
    }
}