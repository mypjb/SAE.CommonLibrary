using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging.Nlog;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Test
{
    public class Test : BaseTest
    {
        public Test(ITestOutputHelper output) : base(output)
        {

        }

        [Fact]
        public void ObjectToJson()
        {
            var dic = new Dictionary<string, string>
            {
                {this.GetRandom(),this.GetRandom() },
                {this.GetRandom(),this.GetRandom() }
            };

            this.WriteLine(dic.ToJsonString());
        }

        [Fact]
        public void JsonToXml()
        {
            var xmlDocument = new XmlDocument();

            XDocument document = XDocument.Parse(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
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
    <logger name=""*"" minlevel=""Debug"" writeTo=""FileTarget"" />
  </rules>
</nlog>");

            this.WriteLine(document);
        }
        [Fact]
        public void XmlToJson()
        {
            var json = "{'Document':{'Root':{'Element1':'1','Element2':'2','Element3':'3','Element4':'4','Element5':'5'}}}";
            var config = json.ToObject<LoggingOptions>();
            this.WriteLine(config);
        }


        [Theory]
        [InlineData("10.1.2.3", true)]
        [InlineData("172.16.45.66", true)]
        [InlineData("192.168.101.164", true)]
        [InlineData("182.86.64.128", false)]
        public void Ip(string address, bool hasLocal)
        {
            if (hasLocal)
            {
                Xunit.Assert.True(IPAddress.Parse(address).IsInnerIP());
            }
            else
            {
                Xunit.Assert.False(IPAddress.Parse(address).IsInnerIP());
            }

        }
    }
}
