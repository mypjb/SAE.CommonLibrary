using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Logging;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.AspNetCore.Authorization.Bitmap
{
    /// <summary>
    /// 端点存储器
    /// </summary>
    public interface IEndpointStorage
    {
        /// <summary>
        /// 根据<paramref name="path"/>获取当前终结点索引
        /// </summary>
        /// <param name="path">请求上下文</param>
        /// <param name="method">请求方式</param>
        /// <returns>返回和终结点匹配的索引</returns>
        int GetIndex(string path, string method);

        /// <summary>
        /// 返回端点长度
        /// </summary>
        int Count();
    }
    /// <summary>
    /// 位图端点
    /// </summary>
    public class Endpoint
    {
        /// <summary>
        /// 索引，从1开始。
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 请求方法
        /// </summary>
        public string Method { get; set; }
    }
    /// <summary>
    /// <see cref="IEndpointStorage"/> 默认实现
    /// </summary>
    /// <inheritdoc/>
    public class EndpointStorage : IEndpointStorage
    {
        private readonly ILogging _logging;
        private readonly IEndpointProvider _endpointProvider;
        /// <summary>
        /// 从提供者当中获取位图端点
        /// </summary>
        /// <value></value>
        protected IEnumerable<Endpoint> GetBitmapEndpoints()
        {
            return this._endpointProvider.ListAsync().GetAwaiter().GetResult() ??
                        Enumerable.Empty<Endpoint>();
        }

        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="logging"></param>
        /// <param name="endpointProvider"></param>
        public EndpointStorage(ILogging<EndpointStorage> logging,
                               IEndpointProvider endpointProvider)
        {
            this._logging = logging;
            this._endpointProvider = endpointProvider;
        }

        public int Count()
        {
            return this.GetBitmapEndpoints().Count();
        }

        public int GetIndex(string path, string method)
        {
            int index;

            var key = $"{path}{Constants.BitmapAuthorize.Separator}{method}".ToLower();

            var bitmapEndpoint = this.GetBitmapEndpoints().FirstOrDefault(s => s.Path.Equals(path, StringComparison.OrdinalIgnoreCase) &&
                                                                     (s.Method.Equals(method, StringComparison.OrdinalIgnoreCase) ||
                                                                     s.Method.IsNullOrWhiteSpace()));

            if (bitmapEndpoint == null)
            {
                this._logging.Info($"未找到对应存储位置,key:{key},path:{path},method:{method}");
                index = -1;
            }
            else
            {
                index = bitmapEndpoint.Index;
            }

            return index;
        }
    }
    /// <summary>
    /// 端点存储扩展类
    /// </summary>
    public static class EndpointStorageExtension
    {
        /// <summary>
        /// 依据<paramref name="context"/>从<see cref="IEndpointStorage"/>查询索引s
        /// </summary>
        /// <param name="storage">存储类</param>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        public static int GetIndex(this IEndpointStorage storage, HttpContext context)
        {
            if (context == null)
            {
                return -1;
            }

            var path = string.Empty;

            var endpoint = context.GetEndpoint();

            if (endpoint is RouteEndpoint)
            {
                var routeEndpoint = (RouteEndpoint)endpoint;
                path = routeEndpoint.RoutePattern.RawText;
                path = path.StartsWith('/') ? path : $"/{path}";
                foreach (var kv in routeEndpoint.RoutePattern.RequiredValues)
                {
                    path = path.Replace($"{{{kv.Key}}}", kv.Value?.ToString(), StringComparison.OrdinalIgnoreCase);
                }
            }
            else
            {
                path = endpoint.DisplayName;
            }
            return storage.GetIndex(path, context.Request.Method);
        }
    }
}
