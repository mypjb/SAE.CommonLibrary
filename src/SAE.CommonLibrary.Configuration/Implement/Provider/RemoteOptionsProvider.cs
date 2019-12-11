using SAE.CommonLibrary.Abstract.Responsibility;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Generic;
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
        private readonly ILogging _logging;
        private readonly HttpClient _clinet;
        private readonly Timer _timer;
        protected RemoteOption Option { get; set; }
        public RemoteOptionsProvider(RemoteConfig remoteConfig, ILogging<RemoteOption> logging)
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

                    this._logging.Debug($"从'{url}'拉取配置成功，远程配置版本号'{response.Body.Version}'");

                    if (response.Body.Version > (this.Option?.Version ?? 0))
                    {
                        this._logging.Info($"远程版本'{response.Body.Version}'高于本地'{response.Body.Version}',将本地配置替换为本地");
                        this.Option = response.Body;
                        break;
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

                    this._logging.Error($"从远处'{url}'，拉取配置时，触发异常", ex);
                }
            }
            this._timer.Change(TimeSpan.FromSeconds(this._remoteConfig.UpdateFrequency), Timeout.InfiniteTimeSpan);
        }

        public Task HandleAsync(OptionsContext context)
        {
            throw new NotImplementedException();
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
                        this._logging.Info($"'{url}'");
                        break;
                    }
                    else
                    {
                        throw new SaeException(result);
                    }

                }
                catch (Exception ex)
                {
                    this._logging.Error(ex);
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
            this.UpdateFrequency = 100;
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
        public object Data { get; set; }
    }
}
