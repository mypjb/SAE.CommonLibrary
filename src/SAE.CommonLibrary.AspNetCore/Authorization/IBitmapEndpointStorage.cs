using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    public interface IBitmapEndpointStorage
    {
        /// <summary>
        /// 根据<paramref name="context"/>获取当前终点
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns>返回和终结点匹配的索引</returns>
        int GetIndex(string path);
        /// <summary>
        /// 添加位图终点
        /// </summary>
        /// <param name="endpoint"></param>
        void Add(BitmapEndpoint endpoint);
        /// <summary>
        /// 批量添加位图终点
        /// </summary>
        /// <param name="endpoints"></param>
        void AddRange(IEnumerable<BitmapEndpoint> endpoints);
    }
    /// <summary>
    /// 位图终点
    /// </summary>
    public class BitmapEndpoint
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
    }

    public class BitmapEndpointStorage : IBitmapEndpointStorage
    {
        private readonly ConcurrentDictionary<string,int> _store;
        public BitmapEndpointStorage()
        {
            this._store = new ConcurrentDictionary<string, int>();
        }
        public void Add(BitmapEndpoint endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint.Path))
            {
                return;
            }
            this._store.AddOrUpdate(endpoint.Path.ToLower(), endpoint.Index, (a, b) => endpoint.Index);
        }

        public void AddRange(IEnumerable<BitmapEndpoint> endpoints)
        {
            foreach (var endpoint in endpoints)
            {
                this.Add(endpoint);
            }
        }

        public int GetIndex(string path)
        {
            int index;
            
            path = path.ToLower();

            if(!this._store.TryGetValue(path, out index))
            {
                index = -1;
            }

            return index;
        }
    }

    public static class BitmapEndpointStorageExtension
    {
        public static int GetIndex(this IBitmapEndpointStorage storage,HttpContext context)
        {
            if (context == null) return -1;

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
            return storage.GetIndex(path);
        }
    }
}
