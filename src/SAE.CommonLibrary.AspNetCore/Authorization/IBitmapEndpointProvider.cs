using SAE.CommonLibrary.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    public interface IBitmapEndpointProvider
    {
        Task<IEnumerable<BitmapEndpoint>> FindsAsync(IEnumerable<IPathDescriptor> paths);
    }
}
