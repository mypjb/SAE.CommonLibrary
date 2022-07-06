using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Abstract.Responsibility;
using SAE.CommonLibrary.Database.Responsibility;
using SAE.CommonLibrary.Scope;

namespace SAE.CommonLibrary.Database
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class ScopeDBConnectionFactory : DefaultDBConnectionFactory
    {
        private readonly IScopeWrapper<DBConnectOptions> _scopeWrapper;

        public ScopeDBConnectionFactory(IOptionsMonitor<List<DBConnectOptions>> optionsMonitor,
                                        IResponsibilityProvider<DatabaseResponsibilityContext> provider,
                                        IScopeWrapper<DBConnectOptions> scopeWrapper
                                        ) : base(optionsMonitor, provider)
        {
            this._scopeWrapper = scopeWrapper;
            optionsMonitor.OnChange(s => this._scopeWrapper.Clear());
        }

        protected override Task<DBConnectOptions> GetOptionsAsync(string name)
        {
            return Task.FromResult(this._scopeWrapper.GetService(() => base.GetOptionsAsync(name).GetAwaiter().GetResult()));
        }

    }


}