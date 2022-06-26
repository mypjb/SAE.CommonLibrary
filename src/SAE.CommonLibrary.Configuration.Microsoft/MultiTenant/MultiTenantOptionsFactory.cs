using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setups"></param>
        /// <param name="postConfigures"></param>
        /// <param name="scopeFactory"></param>
        public MultiTenantOptionsFactory(
            IEnumerable<IConfigureOptions<TOptions>> setups,
            IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
            IScopeFactory scopeFactory) : base(setups, postConfigures)
        {
            this._scopeFactory = scopeFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setups"></param>
        /// <param name="postConfigures"></param>
        /// <param name="validations"></param>
        /// <param name="scopeFactory"></param>
        public MultiTenantOptionsFactory(
            IEnumerable<IConfigureOptions<TOptions>> setups,
            IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
            IEnumerable<IValidateOptions<TOptions>> validations,
            IScopeFactory scopeFactory) : base(setups, postConfigures, validations)
        {
            _scopeFactory = scopeFactory;
        }

        protected override TOptions CreateInstance(string name)
        {
            return base.CreateInstance(name);
        }
    }
}