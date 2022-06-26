using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace SAE.CommonLibrary.Configuration.Microsoft.MultiTenant
{
    public class MultiTenantOptionsMonitor<TOptions> : OptionsMonitor<TOptions> where TOptions : class
    {
        private readonly IOptionsMonitorCache<TOptions> _monitorCache;

        public MultiTenantOptionsMonitor(IOptionsFactory<TOptions> factory,
                                         IEnumerable<IOptionsChangeTokenSource<TOptions>> sources,
                                         IOptionsMonitorCache<TOptions> cache,
                                         IOptions<MultiTenantOptions> options,
                                         IConfiguration configuration) : base(factory, sources, cache)
        {
            this._monitorCache = cache;

            var changeToken = configuration.GetSection(options.Value.ConfigurationNodeName)
                                           .GetReloadToken();
                                           

            changeToken?.RegisterChangeCallback(s =>
            {
                this._monitorCache.Clear();
            }, this);
        }

        public override TOptions Get(string name)
        {
            return base.Get(name);
        }
    }
}