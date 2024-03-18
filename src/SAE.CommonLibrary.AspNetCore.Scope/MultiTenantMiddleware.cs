using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Logging;
using SAE.CommonLibrary.Scope;

namespace SAE.CommonLibrary.AspNetCore.Scope
{
    /// <summary>
    /// 多租户中间件
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
        /// <param name="next">下一个管道</param>
        /// <param name="scopeFactory">区域工厂</param>
        /// <param name="logging">日志记录器</param>
        /// <param name="multiTenantService">多租户服务</param>
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
        /// 执行中间件
        /// </summary>
        /// <param name="context">上下文</param>
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
