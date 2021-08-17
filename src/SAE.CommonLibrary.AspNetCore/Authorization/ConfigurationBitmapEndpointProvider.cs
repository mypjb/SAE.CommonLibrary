using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.AspNetCore.Routing;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    public class ConfigurationEndpointOptions
    {
        public const string Option = "authorize";
        public IEnumerable<PathDescriptor> PathDescriptors { get; set; }
    }
    public class ConfigurationBitmapEndpointProvider :AbstractBitmapEndpointProvider,IBitmapEndpointProvider
    {
        public ConfigurationBitmapEndpointProvider(IOptionsMonitor<ConfigurationEndpointOptions> optionsMonitor,
                                                   ILogging<AbstractBitmapEndpointProvider> logging) : base(logging)
        {
            this.PathDescriptors = optionsMonitor.CurrentValue.PathDescriptors;
            optionsMonitor.OnChange(s =>
            {
                this.PathDescriptors = s.PathDescriptors;
            });
        }
    }
}
