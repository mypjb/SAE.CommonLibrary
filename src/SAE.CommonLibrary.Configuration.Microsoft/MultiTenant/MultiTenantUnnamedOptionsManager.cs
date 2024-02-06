using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace SAE.CommonLibrary.Configuration.Microsoft.MultiTenant
{
    /// <summary>
    /// 多租户默认配置项
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <inheritdoc/>
    public class MultiTenantUnnamedOptionsManager<TOptions> : IOptions<TOptions> where TOptions : class
    {
        private readonly IOptionsMonitor<TOptions> _monitor;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="monitor"></param>
        public MultiTenantUnnamedOptionsManager(IOptionsMonitor<TOptions> monitor)
        {
            this._monitor = monitor;
        }
        public TOptions Value 
        {
            get=>this._monitor.CurrentValue;
        }
    }
}