using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using SAE.CommonLibrary.Scope;

namespace SAE.CommonLibrary.Configuration.Microsoft.MultiTenant
{

    /// <inheritdoc/>
    /// <summary>
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class MultiTenantOptionsMonitor<TOptions> : OptionsMonitor<TOptions> where TOptions : class
    {
        private readonly IOptionsFactory<TOptions> _factory;
        private readonly IOptionsMonitorCache<TOptions> _cache;
        private readonly IScopeFactory _scopeFactory;
        private readonly ILogging _logging;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="sources"></param>
        /// <param name="cache"></param>
        /// <param name="configuration"></param>
        /// <param name="scopeFactory"></param>
        /// <param name="logging"></param>
        public MultiTenantOptionsMonitor(IOptionsFactory<TOptions> factory,
                                         IEnumerable<IOptionsChangeTokenSource<TOptions>> sources,
                                         IOptionsMonitorCache<TOptions> cache,
                                         IConfiguration configuration,
                                         IScopeFactory scopeFactory,
                                         ILogging<MultiTenantOptionsMonitor<TOptions>> logging) : base(factory, sources, cache)
        {



            this._factory = factory;
            this._cache = cache;
            this._scopeFactory = scopeFactory;
            this._logging = logging;

            var changeToken = configuration.GetReloadToken();

            changeToken?.RegisterChangeCallback(s =>
            {
                logging.Info("configuration change clear cache begin");
                cache.Clear();
                logging.Info("configuration change clear cache end");
            }, this);

        }

        public override TOptions Get(string name)
        {
            name ??= Options.DefaultName;

            string cacheKey = name;

            var scope = this._scopeFactory.Get();

            if (!scope.Name.IsNullOrWhiteSpace())
            {
                cacheKey = name.IsNullOrWhiteSpace() ? scope.Name : $"{scope.Name}{Constant.ConfigSeparator}{name}";
                this._logging.Debug($"find '{cacheKey}' options name");
            }
            else
            {
                this._logging.Warn($"use root scope {cacheKey}");
            }

            return _cache.GetOrAdd(cacheKey, () => _factory.Create(name));
        }
    }
}