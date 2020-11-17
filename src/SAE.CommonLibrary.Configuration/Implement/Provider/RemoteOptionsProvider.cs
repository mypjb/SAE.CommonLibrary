using Newtonsoft.Json.Linq;
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
        private readonly RemoteOptions _options;
        private readonly Lazy<ILogging<RemoteOptionsProvider>> _logging;
        private readonly HttpClient _clinet;
        private readonly Timer _timer;

        public event Func<Task> OnChange;

        protected RemoteOption Option { get; set; }

        public RemoteOptionsProvider(RemoteOptions options, Lazy<ILogging<RemoteOptionsProvider>> logging)
        {
            this._options = options;
            this._logging = logging;
            this._clinet = new HttpClient().UsePolly();
            this.Pull(true);
            this._timer = new Timer(this.Pull, null, TimeSpan.FromSeconds(this._options.UpdateFrequency), Timeout.InfiniteTimeSpan);
        }
        /// <summary>
        /// 从远程拉取配置
        /// </summary>
        /// <param name="arg"></param>
        protected void Pull(object arg)
        {
            for (var i = 0; i < this._options.Urls.Length; i++)
            {
                var url = this._options.Urls[i];
                url = $"{url}{(url.IndexOf('?') == -1 ? "?" : "&")}{this._options.VersionParameter}={this.Option?.Version}";
                try
                {
                    var responseMessage = this._clinet.GetAsync(url)
                                                 .GetAwaiter()
                                                 .GetResult();
                    responseMessage.EnsureSuccessStatusCode();

                    var remoteOption = responseMessage.AsAsync<RemoteOption>()
                                                      .GetAwaiter()
                                                      .GetResult();


                    if (arg == null)
                        this._logging.Value.Debug($"从'{url}'拉取配置成功，远程配置版本号'{remoteOption.Version}'");

                    if (remoteOption.Version > (this.Option?.Version ?? 0))
                    {
                        if (arg == null)
                            this._logging.Value.Info($"远程版本'{remoteOption.Version}'高于本地'{remoteOption.Version}',将本地配置替换为本地");

                        this.Option = remoteOption;

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
                    if (i + 1 == this._options.Urls.Length)
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
            this._timer.Change(TimeSpan.FromSeconds(this._options.UpdateFrequency), Timeout.InfiniteTimeSpan);
        }

        public async Task HandleAsync(OptionsContext context)
        {
            var jObject = this.Option.Data as JObject;
            var jProperty = jObject.Property(context.Name, StringComparison.CurrentCultureIgnoreCase);
            if (jProperty != null)
            {
                var json = jProperty.ToString();
                context.SetOption(json.ToObject(context.Type));
                context.Provider = this;
            }
        }

        public async Task SaveAsync(string name, object options)
        {
            var postData = new RemotePostData(name, options);

            for (var i = 0; i < this._options.Urls.Length; i++)
            {
                try
                {
                    var url = this._options.Urls[i];
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                    requestMessage.AddJsonContent(postData);
                    var responseMessage = await this._clinet.SendAsync(requestMessage);
                    
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        this._logging.Value.Info($"数据提交至'{url}'");
                        break;
                    }
                    else
                    {
                        var result = await responseMessage.AsAsync<ErrorOutput>();
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
    public class RemoteOptions
    {
        public RemoteOptions()
        {
            this.UpdateFrequency = 10;
            this.client = new HttpClient();
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

        public string VersionParameter { get; set; } = "version";

        private HttpClient client;

        /// <summary>
        /// 请求端
        /// </summary>
        public HttpClient Client
        {
            get => this.client;
            set
            {
                if (value == null) return;

                this.client = value;

                this.OnChangeClient(this.client);
            }
        }

        protected virtual void OnChangeClient(HttpClient httpClient)
        {

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
