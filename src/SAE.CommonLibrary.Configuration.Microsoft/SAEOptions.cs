using System;
using System.Collections.Generic;
using System.Net.Http;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Extension.Middleware;

namespace SAE.CommonLibrary.Configuration
{
    /// <summary>
    /// <see cref="SAEConfigurationSource"/>配置
    /// </summary>
    public class SAEOptions
    {
        /// <summary>
        /// new <see cref="SAEOptions"/>
        /// </summary>
        public SAEOptions()
        {
            this.Client = new HttpClient();
            this.Client.Timeout = TimeSpan.FromMilliseconds(Constants.DefaultClientTimeout);
            this.PollInterval = Constants.DefaultPollInterval;
            this.NextRequestHeaderName = Constants.DefaultNextRequestHeaderName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public SAEOptions(SAEOptions options)
        {
            this.Client = options.Client;
            this.ConfigurationSection = options.ConfigurationSection;
            this.FileName = options.FileName;
            this.FullPath = options.FullPath;
            this.IncludeEndpointConfiguration = options.IncludeEndpointConfiguration;
            this.NextRequestHeaderName = options.NextRequestHeaderName;
            this.OAuth = options.OAuth;
            this.PollInterval = options.PollInterval;
            this.Url = options.Url;
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        /// <value></value>
        public string FileName { get; set; }
        /// <summary>
        /// 完整路径
        /// </summary>
        /// <value></value>
        public string FullPath { get; set; }
        /// <summary>
        /// 备用地址
        /// </summary>
        /// <value></value>
        public string FullPathBackup{get;set;}
        /// <summary>
        /// 远程配置数据地址
        /// </summary>
        /// <value></value>
        public string Url { get; set; }
        /// <summary>
        /// 配置文件轮询时间单位秒(默认: <see cref="Constants.DefaultPollInterval"/>)
        /// </summary>
        /// <value></value>
        public int PollInterval { get; set; }
        /// <summary>
        /// 远程授权配置
        /// </summary>
        /// <value></value>
        public OAuthOptions OAuth { get; set; }
        /// <summary>
        /// 配置节
        /// </summary>
        /// <remarks>
        /// 如果设置了，来自远程的配置将被包装在最外层。多层可以用"."分割
        /// </remarks>
        /// <example>
        /// <code>
        /// var options = new SAEOptions
        /// {
        ///     ConfigurationSection="test.cf",
        ///     Url="http://xxx.xxx"
        /// }
        /// 
        /// // 远程配置:
        /// // {"connectionStrings":"xxxxx","provider":"sql"}
        /// 
        /// var root = this.ConfigurationBuilder(env).AddRemoteSource(options).Build();
        /// 
        /// // 配置结构如下所示:
        /// //{"test":{"cf":{"connectionStrings":"xxxxx","provider":"sql"}}}
        /// 
        /// root.GetSection("test:cf").GetValue<![CDATA[<DbConfiguration>]]>();
        /// 
        /// </code>
        /// </example>
        public string ConfigurationSection { get; set; }

        /// <summary>
        /// 包含其他端点的配置节，默认<see cref="Constants.Config.IncludeEndpointConfiguration"/>
        /// </summary>
        /// <remarks>
        /// 当配置从<see cref="Url"/>第一次完成远程数据加载时，会从拉取配置中读取<see cref="IncludeEndpointConfiguration"/>。
        /// 如果读取到远程访问地址，会在内部注册一个新的<see cref="SAEConfigurationSource"/>对象。
        /// <para>注：该对象可以递归循环获取。</para>
        /// </remarks>
        public string IncludeEndpointConfiguration { get; set; }
        /// <summary>
        /// 当远程请求成功时，此参数用于指定从响应报头获取下一个远程请求的访问地址(默认：<see cref="Constant.DefaultNextRequestHeaderName"/>)
        /// </summary>
        public string NextRequestHeaderName { get; set; }
        private HttpClient client;
        /// <summary>
        /// 用于发送远程请求的<see cref="HttpClient"/>
        /// </summary>
        /// <value></value>
        public HttpClient Client
        {
            get => this.client;
            set
            {
                if (value == null) return;

                this.client = value;
            }
        }
        /// <summary>
        /// 检查配置
        /// </summary>
        internal void Check()
        {
            Assert.Build(this.Url)
                  .NotNullOrWhiteSpace();
            Assert.Build(this.NextRequestHeaderName)
                  .NotNullOrWhiteSpace();
        }

    }
}
