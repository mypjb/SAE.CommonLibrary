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
            this.Document= XDocument.Parse(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<nlog xmlns=""http://www.nlog-project.org/schemas/NLog.xsd""
      xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
      autoReload=""true""
      internalLogLevel=""Warn""
      internalLogFile=""logs/internal-nlog.log"">
  <targets>
    <target xsi:type=""File"" name=""FileTarget""
                 fileName=""logs/${shortdate}.log""
                 layout=""${level}: ${logger}[${threadid}] ${longdate}${newline}       ${message} ${exception:message,stacktrace}""
                 MaxArchiveFiles=""10""/>
  </targets>
  <rules>
    <logger name=""*"" minlevel=""Info"" writeTo=""FileTarget"" />
  </rules>
</nlog>");
        }

        public XDocument Document { get; set; }
    }
}
