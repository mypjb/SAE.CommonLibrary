using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setups"></param>
        /// <param name="postConfigures"></param>
        /// <param name="scopeFactory"></param>
        /// <param name="configuration"></param>
        public MultiTenantOptionsFactory(
            IEnumerable<IConfigureOptions<TOptions>> setups,
            IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
            IScopeFactory scopeFactory,
            IConfiguration configuration) : base(setups, postConfigures)
        {
            this._scopeFactory = scopeFactory;
            this._configuration = configuration;
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
            IConfiguration configuration) : base(setups, postConfigures, validations)
        {
            _scopeFactory = scopeFactory;
            this._configuration = configuration;
        }

        protected override TOptions CreateInstance(string name)
        {
            return base.CreateInstance(name);
        }
    }
}