using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using SAE.CommonLibrary.Logging;
using SAE.CommonLibrary.Scope;

namespace SAE.CommonLibrary.AspNetCore.Scope
{
    /// <summary>
    /// 租户筛选器
    /// </summary>
    public class MultiTenantFilter : IAsyncActionFilter
    {
        private readonly IMultiTenantService _multiTenantService;
        private readonly IScopeFactory _scopeFactory;
        private readonly ILogging _logging;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="multiTenantService">租户服务</param>
        /// <param name="scopeFactory">区域工厂</param>
        /// <param name="logging">日志记录器</param>
        public MultiTenantFilter(IMultiTenantService multiTenantService,
                                 IScopeFactory scopeFactory,
                                 ILogging<MultiTenantFilter> logging)
        {
            this._multiTenantService = multiTenantService;
            this._scopeFactory = scopeFactory;
            this._logging = logging;
        }

        /// <inheritdoc/>
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