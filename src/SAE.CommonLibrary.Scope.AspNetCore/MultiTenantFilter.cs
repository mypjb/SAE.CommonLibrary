using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.Scope.AspNetCore
{
    /// <summary>
    /// <inheritdoc/>
    /// aspnetcore multi tenant filter
    /// </summary>
    public class MultiTenantFilter : IAsyncActionFilter
    {
        private readonly IMultiTenantService _multiTenantService;
        private readonly IScopeFactory _scopeFactory;
        private readonly ILogging _logging;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="multiTenantService"></param>
        /// <param name="scopeFactory"></param>
        /// <param name="logging"></param>
        public MultiTenantFilter(IMultiTenantService multiTenantService,
                                 IScopeFactory scopeFactory,
                                 ILogging<MultiTenantFilter> logging)
        {
            this._multiTenantService = multiTenantService;
            this._scopeFactory = scopeFactory;
            this._logging = logging;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            this._logging.Debug("find tenant identity");

            var tenantId = await this._multiTenantService.GetAsync(context.HttpContext);

            this._logging.Debug($"setting tenant context:'{tenantId}'");

            using (var scope = await this._scopeFactory.GetAsync(tenantId))
            {
                this._logging.Debug($"call request before({scope.Name})");
                await next.Invoke();
                this._logging.Debug($"call request after({scope.Name})");
            }
        }
    }
}