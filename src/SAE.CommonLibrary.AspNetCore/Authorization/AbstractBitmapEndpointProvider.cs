using SAE.CommonLibrary.AspNetCore.Routing;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    public abstract class AbstractBitmapEndpointProvider : IBitmapEndpointProvider
    {
        private readonly ILogging _logging;

        public AbstractBitmapEndpointProvider(ILogging<AbstractBitmapEndpointProvider> logging)
        {
            this._logging = logging;
            this.PathDescriptors = new IPathDescriptor[0];
        }
        private IEnumerable<IPathDescriptor> pathDescriptors;
        /// <summary>
        /// When <see cref="PathDescriptors"/> all indexes are 0,
        /// index calculation will be performed automatically
        /// </summary>
        protected IEnumerable<IPathDescriptor> PathDescriptors
        {
            get => this.pathDescriptors;
            set
            {
                if (value == null) return;

                this.pathDescriptors = value;

                if (this.PathDescriptors.All(s => s.Index == 0))
                {
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
        public virtual Task<IEnumerable<BitmapEndpoint>> FindsAsync(IEnumerable<IPathDescriptor> descriptors)
        {
            var bitmapEndpoints = new List<BitmapEndpoint>();

            this._logging.Info($"Remote Source:{this.PathDescriptors?.ToJsonString()}");

            this._logging.Info($"Local Target:{descriptors.ToJsonString()}");

            foreach (var descriptor in descriptors)
            {
                var pathDescriptor = this.PathDescriptors?.FirstOrDefault(s =>
                                                        s.Group.Equals(descriptor.Group, StringComparison.OrdinalIgnoreCase) &&
                                                        s.Path.Equals(descriptor.Path, StringComparison.OrdinalIgnoreCase) &&
                                                        s.Method.Equals(descriptor.Method, StringComparison.OrdinalIgnoreCase));
                if (pathDescriptor != null)
                {
                    bitmapEndpoints.Add(new BitmapEndpoint
                    {
                        Index = pathDescriptor.Index,
                        Method = pathDescriptor.Method,
                        Name = pathDescriptor.Name,
                        Path = pathDescriptor.Path
                    });
                }
            }
            this._logging.Info($"BitmapEndpoints:{bitmapEndpoints.ToJsonString()}");
            return Task.FromResult<IEnumerable<BitmapEndpoint>>(bitmapEndpoints);
        }
    }
}
