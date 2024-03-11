using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceProvider serviceProvider;
        private readonly ILogging _logging;
        private IList<IPathDescriptor> pathDescriptors;
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        /// <param name="logging">日志记录器</param>
        public PathDescriptorProvider(IServiceProvider serviceProvider, ILogging<PathDescriptorProvider> logging)
        {
            this.serviceProvider = serviceProvider;
            this._logging = logging;
            this.Scan();
        }
        /// <inheritdoc/>
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

            var actionDescriptorCollectionProvider = serviceProvider.GetService<IActionDescriptorCollectionProvider>();

            if (actionDescriptorCollectionProvider == null)
            {
                var apiDescriptionGroupCollectionProvider = serviceProvider.GetService<IApiDescriptionGroupCollectionProvider>();
                foreach (var group in apiDescriptionGroupCollectionProvider.ApiDescriptionGroups.Items
                                                    .SelectMany(group => group.Items)
                                                    .Where(s => !s.GetType().IsDefined(typeof(ObsoleteAttribute), false)))
                {
                    var name = group.ActionDescriptor.DisplayName;

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
            }
            else
            {
                foreach (var action in actionDescriptorCollectionProvider.ActionDescriptors.Items)
                {
                    var methods = new List<string>();
                    if (action.ActionConstraints != null)
                    {
                        foreach (var actionConstraint in action.ActionConstraints.OfType<HttpMethodActionConstraint>())
                        {
                            foreach (var method in actionConstraint.HttpMethods)
                            {
                                if (!methods.Any(s => s.Equals(method, StringComparison.OrdinalIgnoreCase)))
                                {
                                    methods.Add(method);
                                }
                            }
                        }
                    }
                    else
                    {
                        methods.Add("");
                    }

                    foreach (var method in methods)
                    {
                        var url = action.AttributeRouteInfo.Template.StartsWith('/') ? action.AttributeRouteInfo.Template : $"/{action.AttributeRouteInfo.Template}";
                        foreach (var kv in action.RouteValues)
                        {
                            url = url.Replace($"{{{kv.Key}}}", kv.Value, StringComparison.OrdinalIgnoreCase);
                        }
                        pathDescriptors.Add(new PathDescriptor(action.DisplayName,
                                                               method,
                                                               url,
                                                               string.Empty));
                    }

                }
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
