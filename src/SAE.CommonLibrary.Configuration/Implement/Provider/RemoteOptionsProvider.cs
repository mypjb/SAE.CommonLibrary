﻿using Newtonsoft.Json.Linq;
using SAE.CommonLibrary.Abstract.Responsibility;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Configuration.Implement
{
    /// <summary>
    /// 远程Options提供程序
    /// </summary>
    public class RemoteOptionsProvider : IOptionsProvider
    {
        private readonly RemoteConfig _remoteConfig;
        private readonly Lazy<ILogging<RemoteOptionsProvider>> _logging;
        private readonly HttpClient _clinet;
        private readonly Timer _timer;

        public event Func<Task> OnChange;

        protected RemoteOption Option { get; set; }

        public RemoteOptionsProvider(RemoteConfig remoteConfig, Lazy<ILogging<RemoteOptionsProvider>> logging)
        {
            this._remoteConfig = remoteConfig;
            this._logging = logging;
            this._clinet = new HttpClient().UsePolly();
            this.Pull(true);
            this._timer = new Timer(this.Pull, null, TimeSpan.FromSeconds(this._remoteConfig.UpdateFrequency), Timeout.InfiniteTimeSpan);
        }
        /// <summary>
        /// 从远程拉取配置
        /// </summary>
        /// <param name="arg"></param>
        protected void Pull(object arg)
        {
            for (var i = 0; i < this._remoteConfig.Urls.Length; i++)
            {
                var url = this._remoteConfig.Urls[i];
                try
                {
                    var responseMessage = this._clinet.GetAsync(url)
                                                 .GetAwaiter()
                                                 .GetResult();
                    responseMessage.EnsureSuccessStatusCode();

                    var response = responseMessage.AsAsync<ResponseResult<RemoteOption>>()
                                                  .GetAwaiter()
                                                  .GetResult();

                    if (response.StatusCode != StatusCode.Success)
                    {
                        throw new SaeException(response);
                    }

                    if (arg == null)
                        this._logging.Value.Debug($"从'{url}'拉取配置成功，远程配置版本号'{response.Body.Version}'");

                    if (response.Body.Version > (this.Option?.Version ?? 0))
                    {
                        if (arg == null)
                            this._logging.Value.Info($"远程版本'{response.Body.Version}'高于本地'{response.Body.Version}',将本地配置替换为本地");

                        this.Option = response.Body;

                        if (this.Option.Data.Type == JTokenType.Object)
                        {
                            if (arg == null)
                            {
                                this.OnChange?.Invoke();
                            }
                            break;
                        }
                        else
                        {
                            throw new SaeException($"'{this.Option.Data.Type}'不是一个有效的Json对象");
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (i + 1 == this._remoteConfig.Urls.Length)
                    {
                        if (arg != null)
                        {
                            throw ex;
                        }
                    }

                    if (arg == null)
                        this._logging.Value.Error($"从远处'{url}'，拉取配置时，触发异常", ex);
                }
            }
            this._timer.Change(TimeSpan.FromSeconds(this._remoteConfig.UpdateFrequency), Timeout.InfiniteTimeSpan);
        }

        public async Task HandleAsync(OptionsContext context)
        {
            var jObject = this.Option.Data as JObject;
            var jProperty = jObject.Property(context.Name, StringComparison.CurrentCultureIgnoreCase);
            if (jProperty != null)
            {
                var json = jProperty.ToString();
                context.SetOption(json.ToObject(context.Type));
            }
        }

        public async Task SaveAsync(string name, object options)
        {
            var postData = new RemotePostData(name, options);

            for (var i = 0; i < this._remoteConfig.Urls.Length; i++)
            {
                try
                {
                    var url = this._remoteConfig.Urls[i];
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                    requestMessage.AddJsonContent(postData);
                    var responseMessage = await this._clinet.SendAsync(requestMessage);
                    responseMessage.EnsureSuccessStatusCode();
                    var result = await responseMessage.AsAsync<ResponseResult>();
                    if (result.StatusCode == StatusCode.Success)
                    {
                        this._logging.Value.Info($"数据提交至'{url}'");
                        break;
                    }
                    else
                    {
                        throw new SaeException(result);
                    }

                }
                catch (Exception ex)
                {
                    this._logging.Value.Error(ex);
                }

            }
        }
    }

    /// <summary>
    /// 远程配置
    /// </summary>
    public class RemoteConfig
    {
        public RemoteConfig()
        {
            this.UpdateFrequency = 10;
        }
        private int updateFrequency;

        /// <summary>
        /// 更新频率 默认10s
        /// </summary>
        public int UpdateFrequency
        {
            get => this.updateFrequency;
            set
            {
                if (value > 0)
                {
                    this.updateFrequency = value;
                }
            }
        }
        /// <summary>
        /// 配置节点
        /// </summary>
        public string[] Urls { get; set; }
        /// <summary>
        /// 检查配置
        /// </summary>
        internal void Check()
        {
            Assert.Build(Urls, nameof(Urls))
                  .NotNull()
                  .Then(s => s.Any(), nameof(Urls))
                  .True();
        }
    }
    public class RemotePostData
    {
        public RemotePostData(string name, object data)
        {
            this.Name = name;
            this.Data = data.ToJsonString();
        }
        public string Name { get; }
        public string Data { get; }
    }
    public class RemoteOption
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; set; }
        public JToken Data { get; set; }
    }
}
