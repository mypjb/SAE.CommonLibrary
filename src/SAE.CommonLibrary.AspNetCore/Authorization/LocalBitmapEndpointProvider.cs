using SAE.CommonLibrary.AspNetCore.Routing;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    public class LocalBitmapEndpointProvider : AbstractBitmapEndpointProvider, IBitmapEndpointProvider
    {

        public LocalBitmapEndpointProvider(ILogging<AbstractBitmapEndpointProvider> logging, IPathDescriptorProvider provider) : base(logging)
        {
            this.PathDescriptors = provider.GetDescriptors();
            var index = 0;
            foreach (var item in this.PathDescriptors.OrderBy(s => s.Group)
                                                     .ThenBy(s => s.Path)
                                                     .ThenBy(s => s.Method)
                                                     .ToArray())
            {
                item.Index = ++index;
            }
        }
    }
}
