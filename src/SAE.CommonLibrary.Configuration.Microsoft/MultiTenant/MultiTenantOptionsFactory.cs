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
    public class MultiTenantOptionsFactory<TOptions> : OptionsFactory<TOptions>, IOptionsFactory<TOptions> where TOptions : class
    {
        private readonly IScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly MultiTenantOptions<TOptions> _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setups"></param>
        /// <param name="postConfigures"></param>
        /// <param name="scopeFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="options"></param>
        public MultiTenantOptionsFactory(
            IEnumerable<IConfigureOptions<TOptions>> setups,
            IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
            IScopeFactory scopeFactory,
            IConfiguration configuration,
            IOptions<MultiTenantOptions<TOptions>> options) : base(setups, postConfigures)
        {
            this._scopeFactory = scopeFactory;
            this._configuration = configuration;
            this._options = options.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setups"></param>
        /// <param name="postConfigures"></param>
        /// <param name="validations"></param>
        /// <param name="scopeFactory"></param>
        /// <param name="configuration"></param>
        public MultiTenantOptionsFactory(
            IEnumerable<IConfigureOptions<TOptions>> setups,
            IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
            IEnumerable<IValidateOptions<TOptions>> validations,
            IScopeFactory scopeFactory,
            IConfiguration configuration,
            IOptions<MultiTenantOptions<TOptions>> options) : base(setups, postConfigures, validations)
        {
            _scopeFactory = scopeFactory;
            this._configuration = configuration;
            this._options = options.Value;
        }

        protected override TOptions CreateInstance(string name)
        {

            using (var scope = this._scopeFactory.Get())
            {
                if (scope.Name.IsNullOrWhiteSpace())
                {
                    return base.CreateInstance(name);
                }
                else
                {
                    var key = $"{scope.Name}{Constant.ConfigSeparator}{this._options.Key}";

                    var configuration = this._configuration.GetSection(key);

                    if (configuration.Exists())
                    {
                        return configuration.Get<TOptions>();
                    }
                    return base.CreateInstance(name);
                }
            }

        }
    }
}