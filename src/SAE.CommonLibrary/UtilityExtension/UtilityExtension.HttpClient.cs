using Polly;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Extension
{
    public static partial class UtilityExtension
    {

        /// <summary>
        /// 添加请求头
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static HttpRequestMessage AddHeader(this HttpRequestMessage request, string key, string value)
        {
            return request.AddHeader(key, new string[] { value });
        }
        /// <summary>
        /// 添加请求头
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static HttpRequestMessage AddHeader(this HttpRequestMessage request, string key, params string[] values)
        {
            request.Headers.TryAddWithoutValidation(key, values);
            return request;
        }

        /// <summary>
        /// 以字符串的形式添加数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="value"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public static HttpRequestMessage AddContent(this HttpRequestMessage request,
                                                    string value,
                                                    string mediaType = "application/json")
        {
            if (string.IsNullOrWhiteSpace(value)) return request;

            var content = new StringContent(value, Constant.Encoding, mediaType);

            return request.AddContent(content: content);
        }

        /// <summary>
        /// 以键值对的形式添加数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="nameValue"></param>
        /// <returns></returns>
        public static HttpRequestMessage AddContent(this HttpRequestMessage request, IDictionary<string, string> nameValue)
        {
            var content = new FormUrlEncodedContent(nameValue);

            foreach (var nv in nameValue)
            {
                var stringContent = new StringContent(nv.Value, Encoding.UTF8);
                stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = nv.Key
                };
                request.AddContent(content: stringContent);
            }

            return request;
        }

        /// <summary>
        /// 以Json形式添加数据
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static HttpRequestMessage AddJsonContent<TModel>(this HttpRequestMessage request, TModel model) where TModel : class
        {
            var json = model == null ? string.Empty : model.ToJsonString();

            return request.AddContent(value: json);
        }
        /// <summary>
        /// 添加流Content
        /// </summary>
        /// <param name="request"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static HttpRequestMessage AddContent(this HttpRequestMessage request, Stream stream)
        {
            return request.AddContent(content: new StreamContent(stream));
        }

        /// <summary>
        /// 添加文件Content
        /// </summary>
        /// <param name="request"></param>
        /// <param name="fileInfo"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HttpRequestMessage AddContent(this HttpRequestMessage request, FileInfo fileInfo, string name = null)
        {
            if (string.IsNullOrWhiteSpace(name)) name = Guid.NewGuid().ToString();

            var stream = new StreamContent(fileInfo.OpenRead());

            var content = new MultipartFormDataContent();

            content.Add(stream, name, fileInfo.Name);

            return request.AddContent(content: content);
        }

        /// <summary>
        /// 添加HttpContent
        /// </summary>
        /// <param name="request"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static HttpRequestMessage AddContent(this HttpRequestMessage request, HttpContent content)
        {

            if (request.Content != null)
            {
                //当前内容不为null
                MultipartContent multipart;
                if (request.Content is MultipartContent)
                {
                    //当前内容为多内容实例
                    multipart = request.Content as MultipartContent;
                }
                else
                {
                    //初始化一个多内容的实例
                    multipart = new MultipartFormDataContent();
                    //将当前请求内容附加至多内容
                    multipart.Add(request.Content);
                    //多内容附加至内容
                    request.Content = multipart;
                }

                if (content is MultipartContent)
                {
                    //当前传输内容为多内容格式，将其挨个附加至请求从内容中
                    foreach (var ct in content as MultipartContent)
                    {
                        multipart.Add(ct);
                    }
                }
                else
                {
                    //但内容附加
                    multipart.Add(content);
                }

            }
            else
            {
                //单内容
                request.Content = content;
            }

            return request;
        }
        /// <summary>
        /// 添加请求属性集
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Properties"></param>
        /// <returns></returns>
        public static HttpRequestMessage AddProperty(this HttpRequestMessage request, IDictionary<string, object> Properties)
        {
            foreach (var p in Properties)
                request.Properties[p.Key] = p.Value;
            return request;
        }

        /// <summary>
        /// 获取请求报文，如果请求失败则触发<seealso cref="System.Net.Http.HttpRequestException"/>。
        /// 若<typeparamref name="T"/>继承自<seealso cref="ResponseResult"/>时。
        /// 当<seealso cref="ResponseResult.StatusCode"/>!=<seealso cref="StatusCodes.Success"/>则触发<seealso cref="SaeException"/>异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task<T> AsAsync<T>(this HttpResponseMessage response) where T : class
        {
            var json = await response.Content.ReadAsStringAsync();

            return json.ToObject<T>();
        }



        /// <summary>
        /// 添加Polly代理。当请求出现<paramref name="httpStatusCodes"/>或<seealso cref="HttpRequestException"/>异常时.
        /// 将为进行<paramref name="retryCount"/>次重试
        /// </summary>
        /// <param name="httpClient">请求客户端</param>
        /// <param name="retryCount">失败重试次数默认10次</param>
        /// <param name="httpStatusCodes">当出现错误响应码，将进行重试,默认响应码:
        /// <seealso cref="HttpStatusCode.RequestTimeout"/>,
        /// <seealso cref="HttpStatusCode.InternalServerError"/>,
        /// <seealso cref="HttpStatusCode.BadGateway"/>,
        /// <seealso cref="HttpStatusCode.ServiceUnavailable"/>,
        /// <seealso cref="HttpStatusCode.GatewayTimeout"/></param>
        /// <returns></returns>
        public static HttpClient UsePolly(this HttpClient httpClient, int retryCount = 10, params HttpStatusCode[] httpStatusCodes)
        {
            httpStatusCodes = httpStatusCodes ?? new[] {
               HttpStatusCode.RequestTimeout, // 408
               HttpStatusCode.InternalServerError, // 500
               HttpStatusCode.BadGateway, // 502
               HttpStatusCode.ServiceUnavailable, // 503
               HttpStatusCode.GatewayTimeout // 504
            };

            var policy = Policy.Handle<HttpRequestException>()
                               .OrResult<HttpResponseMessage>(
                                    r => httpStatusCodes.Contains(r.StatusCode))
                               .RetryAsync(10);

            var handlerFieldInfo = Utils.Reflection.GetFieldInfo<HttpMessageInvoker>("_handler", BindingFlags.Instance | BindingFlags.NonPublic);

            var httpMessageHandler = (HttpMessageHandler)handlerFieldInfo.GetValue(httpClient);

            handlerFieldInfo.SetValue(httpClient, new ProxyHandler(response => policy.ExecuteAsync(response), httpMessageHandler));

            return httpClient;
        }

        public static HttpClient UseExceptionHandler(this HttpClient httpClient, Func<HttpResponseMessage,Task> handler)
        {
            
            var handlerFieldInfo = Utils.Reflection.GetFieldInfo<HttpMessageInvoker>("_handler", BindingFlags.Instance | BindingFlags.NonPublic);

            var httpMessageHandler = (HttpMessageHandler)handlerFieldInfo.GetValue(httpClient);

            handlerFieldInfo.SetValue(httpClient, new ProxyHandler(async (proxy) =>
            {
                var responseMessage = await proxy.Invoke();

                if (!responseMessage.IsSuccessStatusCode)
                {
                    await handler.Invoke(responseMessage);
                }
                
                return responseMessage;
            }, httpMessageHandler));

            return httpClient;
        }

        public static HttpClient UseDefaultExceptionHandler(this HttpClient httpClient)
        {
            httpClient.UseExceptionHandler(async response =>
            {
                var json = await response.Content.ReadAsStringAsync();
                if (json.IsNullOrWhiteSpace())
                {
                    throw new SaeException((int)response.StatusCode, json);
                }
                var output = json.ToObject<ErrorOutput>();
                throw new SaeException(output);
            });
            return httpClient;
        }
        private class ProxyHandler : DelegatingHandler
        {
            private readonly Func<Func<Task<HttpResponseMessage>>, Task<HttpResponseMessage>> _proxy;

            public ProxyHandler(Func<Func<Task<HttpResponseMessage>>, Task<HttpResponseMessage>> proxy, HttpMessageHandler innerHandler) : base(innerHandler)
            {
                this._proxy = proxy;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return this._proxy.Invoke(() => base.SendAsync(request, cancellationToken));
            }
        }
    }
}
