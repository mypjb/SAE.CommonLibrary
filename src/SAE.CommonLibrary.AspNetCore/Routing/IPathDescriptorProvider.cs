using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.AspNetCore.Authorization;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.AspNetCore.Routing
{
    /// <summary>
    /// 路径描述符提供程序
    /// </summary>
    public interface IPathDescriptorProvider
    {
        /// <summary>
        /// 获得所有路径描述符
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPathDescriptor> GetDescriptors();
    }
    /// <summary>
    /// <see cref="IPathDescriptorProvider"/> 内部实现
    /// </summary>
    /// <inheritdoc/>
    internal class PathDescriptorProvider : IPathDescriptorProvider
    {
        private readonly IApiDescriptionGroupCollectionProvider _provider;
        private readonly ILogging _logging;
        private IList<IPathDescriptor> pathDescriptors;
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="provider">api提供者</param>
        /// <param name="logging"></param>
        public PathDescriptorProvider(IApiDescriptionGroupCollectionProvider provider, ILogging<PathDescriptorProvider> logging)
        {
            this._provider = provider;
            this._logging = logging;
            this.Scan();
        }
        public IEnumerable<IPathDescriptor> GetDescriptors()
        {
            return this.pathDescriptors;
        }
        /// <summary>
        /// 扫描当前请求的路径
        /// </summary>
        private void Scan()
        {
            this.pathDescriptors = new List<IPathDescriptor>();
            this._logging.Info("准备扫描本地路径");
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
                                                       string.Empty));
            }
            this.pathDescriptors = this.pathDescriptors
                                      .OrderBy(s => s.Group)
                                      .ThenBy(s => s.Path)
                                      .ThenBy(s => s.Method)
                                      .ToList();


            this.pathDescriptors.ForEach((s, i) =>
            {
                s.Index = ++i;
            });

            this._logging.Info($"扫描完成：{this.pathDescriptors.ToJsonString()}");
        }
    }
}
