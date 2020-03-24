using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAE.CommonLibrary.AspNetCore
{
    /// <summary>
    /// path descriptor provider
    /// </summary>
    public interface IPathDescriptorProvider
    {
        /// <summary>
        /// get all path descriptor
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPathDescriptor> GetDescriptors();
    }

    internal class PathDescriptorProvider : IPathDescriptorProvider
    {
        private readonly IApiDescriptionGroupCollectionProvider _provider;
        private IList<IPathDescriptor> pathDescriptors;
        public PathDescriptorProvider(IApiDescriptionGroupCollectionProvider provider)
        {
            this._provider = provider;
            this.Scan();
        }
        public IEnumerable<IPathDescriptor> GetDescriptors()
        {
            return this.pathDescriptors;
        }
        private void Scan()
        {
            this.pathDescriptors = new List<IPathDescriptor>();
            foreach (var group in this._provider.ApiDescriptionGroups.Items
                                                .SelectMany(group => group.Items)
                                                .Where(s => !s.GetType().IsDefined(typeof(ObsoleteAttribute), false)))
            {
                var url = group.RelativePath;
                foreach (var kv in group.ActionDescriptor.RouteValues)
                {
                    url = url.Replace($"{{{kv.Key}}}", kv.Value, StringComparison.OrdinalIgnoreCase);
                }
                pathDescriptors.Add(new PathDescriptor(group.HttpMethod.ToLower(), url.ToLower()));
            }
        }
    }
}
