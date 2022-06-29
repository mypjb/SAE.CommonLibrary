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
    /// <inheritdoc/>
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class MultiTenantOptionsFactory<TOptions> : OptionsFactory<TOptions>, IOptionsFactory<TOptions> where TOptions : class
    {
        private readonly IScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly IOptionsSnapshot<MultiTenantOptions<TOptions>> _options;
        private readonly ILogging _logging;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setups"></param>
        /// <param name="postConfigures"></param>
        /// <param name="scopeFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="options"></param>
        /// <param name="logging"></param>
        public MultiTenantOptionsFactory(
            IEnumerable<IConfigureOptions<TOptions>> setups,
            IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
            IScopeFactory scopeFactory,
            IConfiguration configuration,
            IOptionsSnapshot<MultiTenantOptions<TOptions>> options,
            ILogging<MultiTenantOptionsFactory<TOptions>> logging) : base(setups, postConfigures)
        {
            this._scopeFactory = scopeFactory;
            this._configuration = configuration;
            this._options = options;
            this._logging = logging;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setups"></param>
        /// <param name="postConfigures"></param>
        /// <param name="validations"></param>
        /// <param name="scopeFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="options"></param>
        public MultiTenantOptionsFactory(IEnumerable<IConfigureOptions<TOptions>> setups,
                                         IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
                                         IEnumerable<IValidateOptions<TOptions>> validations,
                                         IScopeFactory scopeFactory,
                                         IConfiguration configuration,
                                         IOptionsSnapshot<MultiTenantOptions<TOptions>> options,
                                         ILogging<MultiTenantOptionsFactory<TOptions>> logging) : base(setups, postConfigures, validations)
        {
            this._scopeFactory = scopeFactory;
            this._configuration = configuration;
            this._options = options;
            this._logging = logging;
        }

        protected override TOptions CreateInstance(string name)
        {
            // get 'name' multi tenant options
            var options = this._options.Get(name);

            this._logging.Info($"{options.ToJsonString()}");

            var scope = this._scopeFactory.Get();
            
            if (scope.Name.IsNullOrWhiteSpace())
            {
                this._logging.Info("scope is null");
                return base.CreateInstance(name);
            }
            else
            {
                this._logging.Info($"scope : {scope.Name}");

                var end = options.Key.IsNullOrWhiteSpace() ? string.Empty : $"{Constant.ConfigSeparator}{options.Key}";

                var key = $"{options.ConfigurationNodeName}{Constant.ConfigSeparator}{scope.Name}{end}";

                this._logging.Info($"get configuration key: {key}");

                var configuration = this._configuration.GetSection(key);

                if (configuration.Exists())
                {
                    this._logging.Info($"find configuration: {key}");
                    return configuration.Get<TOptions>();
                }
                this._logging.Warn($"not find: {key}");
                return base.CreateInstance(name);
            }
        }
    }
}