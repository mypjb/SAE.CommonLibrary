using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SAE.CommonLibrary.Logging.Nlog
{
    public class LoggingConfig
    {
        public LoggingConfig()
        {
            
        }

        public XDocument Document { get; set; }
    }
}
