using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SAE.Framework.Extension;
using SAE.Framework.Logging;
using SAE.Framework.Scope;

namespace SAE.Framework.Configuration.Microsoft.MultiTenant
{

    /// <summary>
    /// 多租户配置监控器
    /// </summary>
    /// <typeparam name="TOptions">配置类型</typeparam>
    public class MultiTenantOptionsMonitor<TOptions> : OptionsMonitor<TOptions> where TOptions : class
    {
        private readonly IOptionsFactory<TOptions> _factory;
        private readonly IOptionsMonitorCache<TOptions> _cache;
        private readonly IScopeFactory _scopeFactory;
        private readonly ILogging _logging;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="factory">配置工厂</param>
        /// <param name="sources">配置源</param>
        /// <param name="cache">缓存接口</param>
        /// <param name="configuration">配置接口</param>
        /// <param name="scopeFactory">区域接口</param>
        /// <param name="logging">日志记录器</param>
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

            ChangeToken.OnChange(configuration.GetReloadToken, () =>
            {
                logging.Info("configuration change clear cache begin");
                cache.Clear();
                logging.Info("configuration change clear cache end");
            });
        }
        /// <inheritdoc/>
        public override TOptions Get(string name)
        {
            name ??= Options.DefaultName;

            string cacheKey = name;

            var scope = this._scopeFactory.Get();

            if (!scope.Name.IsNullOrWhiteSpace())
            {
                cacheKey = name.IsNullOrWhiteSpace() ? scope.Name : $"{scope.Name}{Constants.ConfigSeparator}{name}";
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