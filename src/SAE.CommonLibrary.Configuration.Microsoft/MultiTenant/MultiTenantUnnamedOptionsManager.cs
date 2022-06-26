using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace SAE.CommonLibrary.Configuration.Microsoft.MultiTenant
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class MultiTenantUnnamedOptionsManager<TOptions> : IOptions<TOptions> where TOptions : class
    {
        public readonly IOptionsMonitor<TOptions> _monitor;
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