using SAE.CommonLibrary.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    public class LocalBitmapEndpointProvider : IBitmapEndpointProvider
    {
        private readonly IEnumerable<BitmapEndpoint> _endpoints;

        public LocalBitmapEndpointProvider(IEnumerable<BitmapEndpoint> endpoints)
        {
            this._endpoints = endpoints;
        }
        public Task<IEnumerable<BitmapEndpoint>> FindALLAsync(IEnumerable<IPathDescriptor> descriptors)
        {
            var endpoints = new List<BitmapEndpoint>(descriptors.Count());

            var bitmapEndpoints = _endpoints.ToList();

            foreach (var descriptor in descriptors)
            {
                var index = bitmapEndpoints.FindIndex(s => s.Path.Equals(descriptor.Path, StringComparison.OrdinalIgnoreCase));
                endpoints.Add(new BitmapEndpoint
                {
                    Path = descriptor.Path,
                    Index = index + 1,
                    Name = descriptor.Name
                });
            }

            return Task.FromResult(endpoints.AsEnumerable());
        }
    }
}
