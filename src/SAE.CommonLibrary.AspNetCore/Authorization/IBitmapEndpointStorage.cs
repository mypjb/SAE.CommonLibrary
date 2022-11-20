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

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    /// <summary>
    /// 端点存储器
    /// </summary>
    public interface IBitmapEndpointStorage
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
    public class BitmapEndpoint
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
    /// <see cref="IBitmapEndpointStorage"/> 默认实现
    /// </summary>
    /// <inheritdoc/>
    public class BitmapEndpointStorage : IBitmapEndpointStorage
    {
        private readonly ILogging _logging;
        private readonly IBitmapEndpointProvider _bitmapEndpointProvider;
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        protected IEnumerable<BitmapEndpoint> BitmapEndpoints
        {
            get => this._bitmapEndpointProvider.ListAsync().GetAwaiter().GetResult();
        }
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="logging"></param>
        /// <param name="bitmapEndpointProvider"></param>
        public BitmapEndpointStorage(ILogging<BitmapEndpointStorage> logging,
                                     IBitmapEndpointProvider bitmapEndpointProvider)
        {
            this._logging = logging;
            this._bitmapEndpointProvider = bitmapEndpointProvider;
        }

        public int Count()
        {
            return this.BitmapEndpoints.Count();
        }

        public int GetIndex(string path, string method)
        {
            int index;

            var key = $"{path}{Constants.BitmapAuthorize.Separator}{method}".ToLower();

            var bitmapEndpoint = this.BitmapEndpoints.FirstOrDefault(s => s.Path.Equals(path, StringComparison.OrdinalIgnoreCase) &&
                                                                     s.Method.Equals(method, StringComparison.OrdinalIgnoreCase));

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
    public static class BitmapEndpointStorageExtension
    {
        /// <summary>
        /// 依据<paramref name="context"/>从<see cref="IBitmapEndpointStorage"/>查询索引s
        /// </summary>
        /// <param name="storage">存储类</param>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        public static int GetIndex(this IBitmapEndpointStorage storage, HttpContext context)
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
