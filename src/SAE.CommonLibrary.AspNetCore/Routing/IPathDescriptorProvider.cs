using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAE.CommonLibrary.Extension;
using Microsoft.Extensions.Configuration;
using SAE.CommonLibrary.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace SAE.CommonLibrary.AspNetCore.Routing
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
        private SystemOptions Options { get; set; }
        public PathDescriptorProvider(IApiDescriptionGroupCollectionProvider provider,
                                      IOptionsMonitor<SystemOptions> optionsMonitor)
        {
            this.Options = optionsMonitor.CurrentValue;
            optionsMonitor.OnChange(option =>
            {
                this.Options = option;
            });
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
                //var groupName = group.GroupName;
                var name = group.ActionDescriptor.DisplayName;
                //if (groupName.IsNullOrWhiteSpace())
                //{
                //    if (!name.IsNullOrWhiteSpace() &&
                //        name.EndsWith(')'))
                //    {
                //        groupName = name.TrimEnd(')');
                //        var index = groupName.LastIndexOf("(");
                //        groupName = groupName.Substring(index + 1);
                //        name = name.Substring(0, index);
                //    }
                //    else
                //    {
                //        groupName = "default";
                //    }
                //}
                var url = group.RelativePath;
                foreach (var kv in group.ActionDescriptor.RouteValues)
                {
                    url = url.Replace($"{{{kv.Key}}}", kv.Value, StringComparison.OrdinalIgnoreCase);
                }
                pathDescriptors.Add(new PathDescriptor(name,
                                                       group.HttpMethod,
                                                       url,
                                                       this.Options.Id));
            }
            this.pathDescriptors = this.pathDescriptors
                                      .OrderBy(s => s.Group)
                                      .ThenBy(s => s.Path)
                                      .ToList();
        }
    }
}
