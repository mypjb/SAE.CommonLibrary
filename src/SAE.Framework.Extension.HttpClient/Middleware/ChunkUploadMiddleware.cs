using Microsoft.Net.Http.Headers;
using SAE.Framework.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SAE.Framework.Extension.Middleware
{
    /// <summary>
    /// 分段上传处理程序
    /// </summary>
    public class ChunkUploadMiddleware : DelegatingHandler
    {
        private readonly Func<ILogging> _loggingRecord;
        ///<inheritdoc/>
        public ChunkUploadMiddleware(HttpMessageHandler innerHandler) : base(innerHandler)
        {
            this._loggingRecord = HttpClientExtension.LoggerRecord;
        }
        ///<inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!(request.Content is ChunkStreamContent))
            {
                return await base.SendAsync(request, cancellationToken);
            }
            this._loggingRecord?.Invoke().Info($"请求体属于分段上传'{nameof(ChunkStreamContent)}',开始进入分段上传流程！");
            return await this.UploadCoreAsync(request, (ChunkStreamContent)request.Content, cancellationToken);
        }

        /// <summary>
        /// 上传核心处理程序
        /// </summary>
        /// <param name="httpRequestMessage">请求</param>
        /// <param name="chunkStreamContent">分段流</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>响应正文</returns>
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

            this._loggingRecord?.Invoke().Info($"分段上传-文件名:'{chunkStreamContent.FileName}',文件大小:'{fileStream.Length}',块大小:'{chunkSize}',块数量:'{chunkNum}'");

            HttpResponseMessage httpResponse = null;
            var cookies = new List<string>();
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

                content.Headers.TryAddWithoutValidation(HeaderNames.Cookie, cookies);

                foreach (var header in chunkStreamContent.Headers)
                {
                    content.Headers.Add(header.Key, header.Value);
                }

                content.Headers.ContentRange = new System.Net.Http.Headers.ContentRangeHeaderValue(start, end, fileStream.Length);

                content.Add(new ByteArrayContent(bytes), Path.GetFileNameWithoutExtension(chunkStreamContent.FileName), chunkStreamContent.FileName);

                var requestMessage = httpRequestMessage;

                requestMessage.Content = content;

                var progress = (int)(Math.Round((i + 1.0) / chunkNum, 2) * 100);
                this._loggingRecord?.Invoke().Debug($"文件'{chunkStreamContent.FileName}'上传进度'{progress}%',第{i}块----------{start}~{end}-{size}/{fileStream.Length}");

                httpResponse = await base.SendAsync(requestMessage, cancellationToken);

                httpResponse.EnsureSuccessStatusCode();

                if (httpResponse.Headers.Contains(HeaderNames.SetCookie))
                {
                    IEnumerable<string> sv = httpResponse.Headers.GetValues(HeaderNames.SetCookie)
                                                         .Select(s => s.Substring(0, s.IndexOf(';')));
                    cookies.AddRange(sv);
                };
            }

            this._loggingRecord?.Invoke().Info($"文件'{chunkStreamContent.FileName}'上传完成!");

            return httpResponse;
        }
    }
}
