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
        private readonly IEnumerable<IABACAuthorizationContextProvider> _contextProviders;
        private readonly IEnumerable<IABACAuthorizationDescriptorProvider> _descriptorProviders;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logging"></param>
        /// <param name="providers"></param>
        public ABACAuthorizationHandler(ILogging<ABACAuthorizationHandler> logging,
                                        IEnumerable<IABACAuthorizationContextProvider> contextProviders,
                                        IEnumerable<IABACAuthorizationDescriptorProvider> descriptorProviders)
        {
            this._logging = logging;
            this._contextProviders = contextProviders;
            this._descriptorProviders = descriptorProviders;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ABACAuthorizationRequirement requirement)
        {
            var ctx = new ABACAuthorizationContext();

            foreach (var _contextProvider in this._contextProviders)
            {
                ctx.Add(await _contextProvider.GetAsync());
            }

            foreach (var descriptorProvider in this._descriptorProviders)
            {
                var descriptor = await descriptorProvider.GetAsync();
                if (await descriptor.AuthAsync(ctx))
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }
    }
}