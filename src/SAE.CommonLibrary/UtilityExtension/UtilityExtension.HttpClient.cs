using Microsoft.Net.Http.Headers;
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
        private static Action<string> InfoRecord { get; set; }
        private static HttpStatusCode[] DefaultStatusCodes { get; } = new[] {
               HttpStatusCode.RequestTimeout, // 408
               HttpStatusCode.InternalServerError, // 500
               HttpStatusCode.BadGateway, // 502
               HttpStatusCode.ServiceUnavailable, // 503
               HttpStatusCode.GatewayTimeout // 504
        };
        private static int DefaultTimeout { get; } = 1000 * 30;

        /// <summary>
        /// 使用记录器
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        public static HttpClient UseRecord(this HttpClient httpClient,Action<string> record)
        {
            InfoRecord = record;
            return httpClient;
        }

        /// <summary>
        /// 克隆<paramref name="request"/>请求体
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static HttpRequestMessage Clone(this HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri);
            clone.Content = request.Content;
            clone.Version = request.Version;

            foreach (KeyValuePair<string, object> prop in request.Properties)
            {
                clone.Properties.Add(prop);
            }

            foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            return clone;
        }

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
                stringContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
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

        #region Middleware

        /// <summary>
        /// 使用默认的中间件
        /// </summary>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        public static HttpClient UseDefaultMiddleware(this HttpClient httpClient)
        {
            httpClient.Timeout = TimeSpan.FromMilliseconds(DefaultTimeout);
            return httpClient.UsePolly()
                             .UseChunkHandler();
        }

        /// <summary>
        /// 使用<paramref name="handler"/>作为中间件
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static HttpClient Use(this HttpClient httpClient, HttpMessageHandler handler)
        {
            var handlerFieldInfo = Utils.Reflection.GetFieldInfo<HttpMessageInvoker>("_handler", BindingFlags.Instance | BindingFlags.NonPublic);

            //var httpMessageHandler = (HttpMessageHandler)handlerFieldInfo.GetValue(httpClient);

            handlerFieldInfo.SetValue(httpClient, new ProxyHandler(handler));

            return httpClient;
        }

        /// <summary>
        /// 使用<paramref name="handler"/>作为中间件
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static HttpClient Use(this HttpClient httpClient, Action<HttpRequestMessage> requestProxy)
        {
            var handlerFieldInfo = Utils.Reflection.GetFieldInfo<HttpMessageInvoker>("_handler", BindingFlags.Instance | BindingFlags.NonPublic);

            var httpMessageHandler = (HttpMessageHandler)handlerFieldInfo.GetValue(httpClient);

            handlerFieldInfo.SetValue(httpClient, new ProxyHandler(requestProxy, httpMessageHandler));

            return httpClient;
        }


        /// <summary>
        /// 使用<paramref name="proxy"/>作为响应中间件
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static HttpClient Use(this HttpClient httpClient, Func<Func<Task<HttpResponseMessage>>, Task<HttpResponseMessage>> responseProxy)
        {
            var handlerFieldInfo = Utils.Reflection.GetFieldInfo<HttpMessageInvoker>("_handler", BindingFlags.Instance | BindingFlags.NonPublic);

            var httpMessageHandler = (HttpMessageHandler)handlerFieldInfo.GetValue(httpClient);

            handlerFieldInfo.SetValue(httpClient, new ProxyHandler(responseProxy, httpMessageHandler));

            return httpClient;
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
            httpStatusCodes = httpStatusCodes ?? DefaultStatusCodes;

            var policy = Policy.Handle<HttpRequestException>()
                               .OrResult<HttpResponseMessage>(
                                    r => httpStatusCodes.Contains(r.StatusCode))
                               .RetryAsync(10);

            return httpClient.Use(response => policy.ExecuteAsync(response));
        }

        /// <summary>
        /// 使用<paramref name="handler"/>作为异常处理中间件
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static HttpClient UseExceptionHandler(this HttpClient httpClient, Func<HttpResponseMessage, Task> handler)
        {
            return httpClient.Use(async (proxy) =>
            {
                var responseMessage = await proxy.Invoke();

                if (!responseMessage.IsSuccessStatusCode)
                {
                    await handler.Invoke(responseMessage);
                }

                return responseMessage;
            });
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
        /// <summary>
        /// 使用分块上传处理程序
        /// </summary>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        public static HttpClient UseChunkHandler(this HttpClient httpClient)
        {
            var handlerFieldInfo = Utils.Reflection.GetFieldInfo<HttpMessageInvoker>("_handler", BindingFlags.Instance | BindingFlags.NonPublic);

            var httpMessageHandler = (HttpMessageHandler)handlerFieldInfo.GetValue(httpClient);
            return httpClient.Use(new ChunkMessageHandler(httpMessageHandler));
        }

        #endregion

        #region HttpClient

        public static async Task<Stream> DownloadAsync(this HttpClient httpClient,
                                                       HttpRequestMessage httpRequestMessage,
                                                       long chunkSize = ChunkStreamContent.DefaultChunkSize)
        {
            ///第一次嗅探请求
            httpRequestMessage.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(0, 0);

            var responseMessage = await httpClient.SendAsync(httpRequestMessage);

            responseMessage.EnsureSuccessStatusCode();


            var responseStream = new MemoryStream();

            var length = responseMessage.Content.Headers.ContentRange.Length.Value;

            var chunkNum = length / chunkSize;

            if (length % chunkSize != 0)
            {
                chunkNum++;
            }

            InfoRecord?.Invoke($"分段下载-文件名:'{httpRequestMessage.RequestUri}',文件大小:'{length}',块大小:'{chunkSize}',块数量:'{chunkNum}'");

            for (int i = 0; i < chunkNum; i++)
            {
                var req = httpRequestMessage.Clone();
                var start = i * chunkSize;
                var end = (i + 1 == chunkNum ? length : (i + 1) * chunkSize) - 1;

                req.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(start, end);

                var httpResponse = await httpClient.SendAsync(req);

                httpResponse.EnsureSuccessStatusCode();

                await (await httpResponse.Content.ReadAsStreamAsync()).CopyToAsync(responseStream);
                var progress = (int)Math.Round((i + 1.0) / chunkNum, 2) * 100;
                InfoRecord?.Invoke($"文件'{req.RequestUri}'上传进度'{progress}%',第{i}块----------{start}~{end}/{length}");
            }
            responseStream.Position = 0;
            return responseStream;
        }

        #endregion

        #region Private Class

        private class ProxyHandler : DelegatingHandler
        {
            private readonly Func<Func<Task<HttpResponseMessage>>, Task<HttpResponseMessage>> _responseProxy;
            private readonly Action<HttpRequestMessage> _requestProxy;
            public ProxyHandler(HttpMessageHandler innerHandler) : base(innerHandler)
            {
            }

            public ProxyHandler(Action<HttpRequestMessage> requestProxy, HttpMessageHandler innerHandler) : this(innerHandler)
            {
                this._requestProxy = requestProxy;
            }

            public ProxyHandler(Func<Func<Task<HttpResponseMessage>>, Task<HttpResponseMessage>> proxy, HttpMessageHandler innerHandler) : this(innerHandler)
            {
                this._responseProxy = proxy;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (this._requestProxy != null)
                {
                    this._requestProxy.Invoke(request);
                }

                if (this._responseProxy == null)
                {
                    return base.SendAsync(request, cancellationToken);
                }
                else
                {
                    return this._responseProxy.Invoke(() => base.SendAsync(request, cancellationToken));
                }
            }
        }

        /// <summary>
        /// 分段上传处理程序
        /// </summary>
        private class ChunkMessageHandler : DelegatingHandler
        {

            public ChunkMessageHandler(HttpMessageHandler innerHandler) : base(innerHandler)
            {
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (!(request.Content is ChunkStreamContent))
                {
                    return await base.SendAsync(request, cancellationToken);
                }
                InfoRecord?.Invoke($"请求体属于分段上传'{nameof(ChunkStreamContent)}',开始进入分段上传流程！");
                return await this.UploadCoreAsync(request, (ChunkStreamContent)request.Content, cancellationToken);
            }


            private async Task<HttpResponseMessage> UploadCoreAsync(HttpRequestMessage httpRequestMessage,
                                                                    ChunkStreamContent chunkStreamContent,
                                                                    CancellationToken cancellationToken)
            {

                var chunkSize = chunkStreamContent.ChunkSize;

                var fileStream = await chunkStreamContent.ReadAsStreamAsync();

                var chunkNum = fileStream.Length / chunkSize;

                if (fileStream.Length % chunkSize != 0)
                {
                    chunkNum++;
                }

                InfoRecord?.Invoke($"分段上传-文件名:'{chunkStreamContent.FileName}',文件大小:'{fileStream.Length}',块大小:'{chunkSize}',块数量:'{chunkNum}'");

                HttpResponseMessage httpResponse = null;

                for (var i = 0; i < chunkNum; i++)
                {
                    var start = i * chunkSize;
                    var end = (i + 1 == chunkNum ? fileStream.Length : (i + 1) * chunkSize) - 1;
                    var size = chunkSize;
                    if (i + 1 == chunkNum)
                    {
                        size = (int)fileStream.Length - chunkSize * i;
                    }

                    var bytes = new byte[size];

                    await fileStream.ReadAsync(bytes, 0, bytes.Length);

                    var content = new MultipartFormDataContent();

                    foreach (var header in chunkStreamContent.Headers)
                    {
                        content.Headers.Add(header.Key, header.Value);
                    }

                    content.Headers.ContentRange = new System.Net.Http.Headers.ContentRangeHeaderValue(start, end, fileStream.Length);

                    content.Add(new ByteArrayContent(bytes), Path.GetFileNameWithoutExtension(chunkStreamContent.FileName), chunkStreamContent.FileName);

                    var requestMessage = httpRequestMessage;

                    requestMessage.Content = content;

                    var progress = (int)Math.Round((i + 1.0) / chunkNum, 2) * 100;
                    InfoRecord?.Invoke($"文件'{chunkStreamContent.FileName}'上传进度'{progress}%',第{i}块----------{start}~{end}-{size}/{fileStream.Length}");

                    httpResponse = await base.SendAsync(requestMessage, cancellationToken);

                    httpResponse.EnsureSuccessStatusCode();
                }

                InfoRecord?.Invoke($"文件'{chunkStreamContent.FileName}'上传完成!");

                return httpResponse;
            }
        }
        #endregion



    }

    /// <summary>
    /// 分段上传内容流
    /// </summary>
    public class ChunkStreamContent : StreamContent
    {
        /// <summary>
        /// 默认块大小5MB
        /// </summary>
        public const int DefaultChunkSize = 1024 * 1024 * 5;
        /// <summary>
        /// 块大小
        /// </summary>
        public int ChunkSize { get; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; }
        /// <summary>
        /// 创建大块文件流内容
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        public ChunkStreamContent(Stream content, string fileName) : this(content, fileName, DefaultChunkSize)
        {

        }
        /// <summary>
        /// 创建大块文件流内容
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <param name="chunkSize"></param>
        public ChunkStreamContent(Stream stream, string fileName, int chunkSize) : base(stream)
        {
            Assert.Build(stream != null && stream.Length > 0)
                  .True("文件流无效!");
            this.ChunkSize = chunkSize <= 0 ? DefaultChunkSize : chunkSize;
            this.FileName = fileName ?? string.Empty;
            this.Headers.Add(HeaderNames.ContentMD5, stream.ToMd5());
        }

    }
}
