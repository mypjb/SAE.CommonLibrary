using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.AspNetCore.Routing;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.AspNetCore.Authorization.Bitmap
{
    /// <summary>
    /// 端点配置类
    /// </summary>
    public class ConfigurationEndpointOptions
    {
        /// <summary>
        /// 配置节
        /// </summary>
        public const string Option = Constants.BitmapAuthorize.Option;
        /// <summary>
        /// 路径描述
        /// </summary>
        /// <value></value>
        public IEnumerable<Endpoint> BitmapEndpoints { get; set; }
    }
    /// <summary>
    /// 基于<see cref="IConfiguration"/>、<see cref="IOptions{ConfigurationEndpointOptions}"/>端点提供者。
    /// </summary>
    /// <inheritdoc/>
    public class ConfigurationEndpointProvider :AbstractEndpointProvider,IEndpointProvider
    {
        /// <summary>
        /// 创建一个新的对象
        /// </summary>
        /// <param name="optionsMonitor">监控器</param>
        /// <param name="logging">日志记录器</param>
        public ConfigurationEndpointProvider(IOptionsMonitor<ConfigurationEndpointOptions> optionsMonitor,
                                                   ILogging<ConfigurationEndpointProvider> logging) : base(logging)
        {
            this.Endpoints = optionsMonitor.CurrentValue.BitmapEndpoints;
            optionsMonitor.OnChange(s =>
            {
                this.Endpoints = s.BitmapEndpoints;
            });
        }
    }
}
