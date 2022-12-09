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
    /// <typeparam name="TOptions"></typeparam>
    /// <inheritdoc/>
    public class MultiTenantOptionsManager<TOptions> : OptionsManager<TOptions>, IOptionsSnapshot<TOptions> where TOptions : class
    {
        private readonly IOptionsMonitor<TOptions> _monitor;

        public MultiTenantOptionsManager(IOptionsFactory<TOptions> factory,
                                         IOptionsMonitor<TOptions> monitor) : base(factory)
        {
            this._monitor = monitor;
        }

        public override TOptions Get(string name)
        {
            return this._monitor.Get(name);
        }
    }
}