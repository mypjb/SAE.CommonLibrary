using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SAE.CommonLibrary.AspNetCore.Authorization.ABAC;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    /// <summary>
    /// ABAC授权认证的实现
    /// </summary>
    public class ABACAuthorizationHandler : AuthorizationHandler<ABACAuthorizationRequirement>
    {
        private readonly ILogging _logging;
        private readonly IEnumerable<IABACAuthorizationContextProvider> _providers;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logging"></param>
        /// <param name="providers"></param>
        public ABACAuthorizationHandler(ILogging<ABACAuthorizationHandler> logging,
                                        IEnumerable<IABACAuthorizationContextProvider> providers)
        {
            this._logging = logging;
            this._providers = providers;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ABACAuthorizationRequirement requirement)
        {
            var ctx = new ABACAuthorizationContext();

            foreach (var provider in this._providers)
            {
                ctx.Add(await provider.GetAsync());
            }

            
        }
    }
}