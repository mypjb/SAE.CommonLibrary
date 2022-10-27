using System;
using System.Net.Http;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Extension.Middleware;

namespace SAE.CommonLibrary.Configuration
{
    /// <summary>
    /// sae source options
    /// </summary>
    public class SAEOptions
    {
        /// <summary>
        /// new <see cref="SAEOptions"/>
        /// </summary>
        public SAEOptions()
        {
            this.Client = new HttpClient();
            this.Client.Timeout = TimeSpan.FromMilliseconds(Constant.DefaultClientTimeout);
            this.PollInterval = Constant.DefaultPollInterval;
            this.NextRequestHeaderName = Constant.DefaultNextRequestHeaderName;
        }
        /// <summary>
        /// path to the persisted configuration file
        /// </summary>
        /// <value></value>
        public string FileName { get; set; }
        /// <summary>
        /// remote configuration address
        /// </summary>
        /// <value></value>
        public string Url { get; set; }
        /// <summary>
        /// configure the rotation training time（unit seconds）
        /// </summary>
        /// <value></value>
        public int PollInterval { get; set; }
        /// <summary>
        /// remote configuration auth configuration
        /// </summary>
        /// <value></value>
        public OAuthOptions OAuth { get; set; }
        /// <summary>
        /// configuration section
        /// </summary>
        /// <remarks>
        /// if set, the configuration from the remote is wrapped in the outermost layer. Multiple layers can be used with "."
        /// </remarks>
        /// <example>
        /// <code>
        /// var options = new SAEOptions
        /// {
        ///     ConfiguraionSection="test.cf",
        ///     Url="http://xxx.xxx"
        /// }
        /// 
        /// // remote configuration:
        /// // {"connectionStrings":"xxxxx","provider":"sql"}
        /// 
        /// var root = this.ConfigurationBuilder(env).AddRemoteSource(options).Build();
        /// 
        /// // the configuration has the following structure:
        /// //{"test":{"cf":{"connectionStrings":"xxxxx","provider":"sql"}}}
        /// 
        /// root.GetSection("test:cf").GetValue<![CDATA[<DbConfiguration>]]>();
        /// 
        /// </code>
        /// </example>
        public string ConfiguraionSection { get; set; }
        /// <summary>
        /// when the remote request is successful, this parameter is used to specify the access address to get the next remote request from the response header
        /// </summary>
        public string NextRequestHeaderName { get; set; }
        private HttpClient client;
        /// <summary>
        /// send remote request client 
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
        /// check options
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
