using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Scope;

namespace SAE.CommonLibrary.Configuration.Microsoft.MultiTenant
{

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class MultiTenantOptionsMonitor<TOptions> : OptionsMonitor<TOptions> where TOptions : class
    {
        public MultiTenantOptionsMonitor(IOptionsFactory<TOptions> factory,
                                         IEnumerable<IOptionsChangeTokenSource<TOptions>> sources,
                                         IOptionsMonitorCache<TOptions> cache,
                                         IOptions<MultiTenantOptions<TOptions>> options,
                                         IConfiguration configuration) : base(factory, sources, cache)
        {
            var changeToken = configuration.GetSection(options.Value.ConfigurationNodeName)
                                           .GetReloadToken();

            changeToken?.RegisterChangeCallback(s =>
            {
                cache.Clear();
            }, this);
        }
    }
}