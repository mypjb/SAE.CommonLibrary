using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.Scope.AspNetCore
{
    /// <summary>
    /// multi tenant middleware
    /// </summary>
    public class MultiTenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IScopeFactory _scopeFactory;
        private readonly ILogging _logging;
        private readonly IMultiTenantService _multiTenantService;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="scopeFactory"></param>
        /// <param name="logging"></param>
        /// <param name="multiTenantService"></param>
        public MultiTenantMiddleware(RequestDelegate next,
                                     IScopeFactory scopeFactory,
                                     ILogging<MultiTenantMiddleware> logging,
                                     IMultiTenantService multiTenantService)
        {
            this._next = next;
            this._scopeFactory = scopeFactory;
            this._logging = logging;
            this._multiTenantService = multiTenantService;
        }

        /// <summary>
        /// call middleware request
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            this._logging.Debug("find tenant identity");

            var tenantId = await this._multiTenantService.GetAsync(context);

            this._logging.Debug($"setting tenant context:'{tenantId}'");

            using (var scope = await this._scopeFactory.GetAsync(tenantId))
            {
                this._logging.Debug($"call request before({scope.Name})");
                await this._next.Invoke(context);
                this._logging.Debug($"call request after({scope.Name})");
            }
        }
    }
}
