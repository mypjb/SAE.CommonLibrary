using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.Scope.AspNetCore
{
    /// <summary>
    /// <para>aspnetcore tenant service </para>
    /// <para>
    /// The tenant identity is obtained from the context according to the policy
    /// </para>
    /// </summary>
    public interface IMultiTenantService
    {
        /// <summary>
        /// get tenant identity from <paramref name="ctx"/> 
        /// </summary>
        /// <param name="ctx">request context</param>
        /// <returns>return tenant identity</returns>
        Task<string> GetAsync(HttpContext ctx);
    }

    /// <summary>
    /// <inheritdoc/>
    /// default <see cref="IMultiTenantService"/> imp
    /// </summary>
    public class DefaultMultiTenantService : IMultiTenantService
    {
        /// <summary>
        /// domain separator char
        /// </summary>
        private const char DomainSeparator = '.';
        /// <summary>
        /// aspnetcore multi tenant options
        /// </summary>
        private MultiTenantOptions Options;
        /// <summary>
        /// logging
        /// </summary>
        private readonly ILogging _logging;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="monitor"></param>
        /// <param name="logging"></param>
        public DefaultMultiTenantService(IOptionsMonitor<MultiTenantOptions> monitor,
                                         ILogging<DefaultMultiTenantService> logging)
        {
            this._logging = logging;
            this.OnChange(monitor.CurrentValue);
            monitor.OnChange(this.OnChange);
        }

        /// <summary>
        /// listener configuration change
        /// </summary>
        /// <param name="options"></param>
        private void OnChange(MultiTenantOptions options)
        {
            this._logging.Info($"configuration tenant :\r\n{options.ToJsonString()}");
            this.Options = options;
        }
        
        public Task<string> GetAsync(HttpContext ctx)
        {
            var tenantId = string.Empty;
            if (this.Options.Strategy == MultiTenantStrategy.Domain)
            {
                tenantId = this.DomainFind(ctx);
            }
            else if (this.Options.Strategy == MultiTenantStrategy.User)
            {
                tenantId = this.UserFind(ctx);
            }

            if (tenantId.IsNullOrWhiteSpace())
            {
                tenantId = this.UserFind(ctx);
            }

            return Task.FromResult(tenantId);
        }

        private string DomainFind(HttpContext ctx)
        {
            var hostString = ctx.Request.Host;

            var host = hostString.Host;

            var index = -1;

            if (this.Options.UseDefaultRule &&
                (string.IsNullOrWhiteSpace(this.Options.Host) ||
                (index = host.IndexOf(this.Options.Host)) < 1))
            {
                this._logging.Warn($"The current domain({host}) name does not match the domain({this.Options.Host}) name in the configuration");
                index = host.IndexOf(DomainSeparator);
            }

            var tenantCode = index == -1 ? host : host.Substring(0, index);

            this._logging.Debug($"({host}) match code ({tenantCode})");

            if (!this.Options.Mapper.TryGetValue(tenantCode, out var tenantId))
            {
                this._logging.Warn($"Identity matching tenant code({tenantCode}) not found");
                tenantId = tenantCode;
            }

            return tenantId;

        }

        private string UserFind(HttpContext ctx)
        {
            var tenantId = string.Empty;

            if (ctx.User?.Identity?.IsAuthenticated ?? false)
            {
                this._logging.Debug($"用户认证通过,尝试从用户属性({this.Options.ClaimName})获取租户信息");
                var tenant = ctx.User.FindFirstValue(this.Options.ClaimName);
                tenantId = tenant;
            }

            return tenantId;
        }

        private string HeaderFind(HttpContext ctx)
        {
            var tenantId = string.Empty;
            StringValues sv;
            if (ctx.Request.Headers.TryGetValue(this.Options.ClaimName, out sv))
            {
                tenantId = sv.First();
            }
            return tenantId;
        }
    }
}