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
    /// <summary>
    /// 多租户配置工厂
    /// </summary>
    /// <typeparam name="TOptions">配置类型</typeparam>
    public class MultiTenantOptionsFactory<TOptions> : OptionsFactory<TOptions>, IOptionsFactory<TOptions> where TOptions : class
    {
        private readonly IScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly IOptionsMonitor<MultiTenantOptions<TOptions>> _optionsMonitor;
        private readonly ILogging _logging;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="setups">启动时的配置函数</param>
        /// <param name="postConfigures">初始化后的配置函数</param>
        /// <param name="scopeFactory">区域接口</param>
        /// <param name="configuration">配置接口</param>
        /// <param name="optionsMonitor">多租户配置监控器</param>
        /// <param name="logging">日志记录器</param>
        public MultiTenantOptionsFactory(
            IEnumerable<IConfigureOptions<TOptions>> setups,
            IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
            IScopeFactory scopeFactory,
            IConfiguration configuration,
            IOptionsMonitor<MultiTenantOptions<TOptions>> optionsMonitor,
            ILogging<MultiTenantOptionsFactory<TOptions>> logging) : base(setups, postConfigures)
        {
            this._scopeFactory = scopeFactory;
            this._configuration = configuration;
            this._optionsMonitor = optionsMonitor;
            this._logging = logging;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="setups">启动时的配置函数</param>
        /// <param name="postConfigures">初始化后的配置函数</param>
        /// <param name="validations">验证接口</param>
        /// <param name="scopeFactory">区域接口</param>
        /// <param name="configuration">配置接口</param>
        /// <param name="optionsMonitor">多租户配置监控器</param>
        /// <param name="logging">日志记录器</param>
        public MultiTenantOptionsFactory(IEnumerable<IConfigureOptions<TOptions>> setups,
                                         IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
                                         IEnumerable<IValidateOptions<TOptions>> validations,
                                         IScopeFactory scopeFactory,
                                         IConfiguration configuration,
                                         IOptionsMonitor<MultiTenantOptions<TOptions>> optionsMonitor,
                                         ILogging<MultiTenantOptionsFactory<TOptions>> logging) : base(setups, postConfigures, validations)
        {
            this._scopeFactory = scopeFactory;
            this._configuration = configuration;
            this._optionsMonitor = optionsMonitor;
            this._logging = logging;
        }
        /// <inheritdoc/>
        protected override TOptions CreateInstance(string name)
        {
            // get 'name' multi tenant options
            var options = this._optionsMonitor.Get(name);

            this._logging.Info($"{options.ToJsonString()}");

            var scope = this._scopeFactory.Get();

            if (!scope.Name.IsNullOrWhiteSpace())
            {
                this._logging.Info($"scope : {scope.Name}");

                var end = options.Key.IsNullOrWhiteSpace() ? string.Empty : $"{Constants.ConfigSeparator}{options.Key}";

                var key = $"{options.ConfigurationNodeName}{Constants.ConfigSeparator}{scope.Name}{end}";

                this._logging.Info($"get configuration key: {key}");

                var configuration = this._configuration.GetSection(key);

                if (configuration.Exists())
                {
                    this._logging.Info($"find configuration: {key}");
                    return configuration.Get<TOptions>();
                }
                this._logging.Warn($"not find: {key}");
            }
            else
            {
                this._logging.Info("not exist scope");
            }

            if (!options.Key.IsNullOrWhiteSpace())
            {
                var configurationSection = this._configuration.GetSection(options.Key);
                if (configurationSection.Exists())
                {
                    this._logging.Info($"find configuration: {options.Key}");
                    return configurationSection.Get<TOptions>();
                }
            }

            this._logging.Warn($"not match configuration:{options.Key}-{name}");

            return base.CreateInstance(name);
        }
    }
}