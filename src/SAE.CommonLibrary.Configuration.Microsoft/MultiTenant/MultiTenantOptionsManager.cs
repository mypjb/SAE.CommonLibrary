using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace SAE.CommonLibrary.Configuration.Microsoft.MultiTenant
{
    /// <summary>
    /// 多租户配置管理
    /// </summary>
    /// <typeparam name="TOptions">配置类型</typeparam>
    /// <inheritdoc/>
    public class MultiTenantOptionsManager<TOptions> : OptionsManager<TOptions>, IOptionsSnapshot<TOptions> where TOptions : class
    {
        private readonly IOptionsMonitor<TOptions> _monitor;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="factory">配置工厂</param>
        /// <param name="monitor">配置监控器</param>
        public MultiTenantOptionsManager(IOptionsFactory<TOptions> factory,
                                         IOptionsMonitor<TOptions> monitor) : base(factory)
        {
            this._monitor = monitor;
        }
        /// <inheritdoc/>
        public override TOptions Get(string name)
        {
            return this._monitor.Get(name);
        }
    }
}