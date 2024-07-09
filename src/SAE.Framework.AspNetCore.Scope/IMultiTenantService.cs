using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SAE.Framework.Extension;
using SAE.Framework.Logging;


namespace SAE.Framework.AspNetCore.Scope
{
    /// <summary>
    /// <para>aspnetcore多租户服务</para>
    /// <para>
    /// 根据策略从上下文中获取租户标识
    /// </para>
    /// </summary>
    public interface IMultiTenantService
    {
        /// <summary>
        /// 根据上下文获取租户信息
        /// </summary>
        /// <param name="ctx">上下文</param>
        /// <returns>租户标识</returns>
        Task<string> GetAsync(HttpContext ctx);
    }

    /// <summary>
    ///  <see cref="IMultiTenantService"/>默认实现
    /// </summary>
    public class DefaultMultiTenantService : IMultiTenantService
    {
        /// <summary>
        /// 域名分隔符
        /// </summary>
        private const char DomainSeparator = '.';
        /// <summary>
        /// 租户配置
        /// </summary>
        private MultiTenantOptions Options;
        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogging _logging;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="monitor">配置对象</param>
        /// <param name="logging">日志记录器</param>
        public DefaultMultiTenantService(IOptionsMonitor<MultiTenantOptions> monitor,
                                         ILogging<DefaultMultiTenantService> logging)
        {
            this._logging = logging;
            this.OnChange(monitor.CurrentValue);
            monitor.OnChange(this.OnChange);
        }

        /// <summary>
        /// 监控配置更改
        /// </summary>
        /// <param name="options"></param>
        private void OnChange(MultiTenantOptions options)
        {
            this._logging.Info(()=> $"configuration tenant :\r\n{options.ToJsonString()}");
            this.Options = options;
        }
        /// <inheritdoc/>
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
                tenantId = this.HeaderFind(ctx);
            }

            return Task.FromResult(tenantId);
        }
        /// <summary>
        /// 在域名中查找租户信息
        /// </summary>
        /// <param name="ctx">上下文</param>
        /// <returns>租户标识</returns>
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
        /// <summary>
        /// 在用户上查找租户信息
        /// </summary>
        /// <param name="ctx">上下文</param>
        /// <returns>租户标识</returns>
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
        /// <summary>
        /// 在请求头上查找租户信息
        /// </summary>
        /// <param name="ctx">上下文</param>
        /// <returns>返回租户信息</returns>
        private string HeaderFind(HttpContext ctx)
        {
            var tenantId = string.Empty;
            StringValues sv;
            if (ctx.Request.Headers.TryGetValue(this.Options.HeaderName, out sv))
            {
                tenantId = sv.First();
            }
            return tenantId;
        }
    }
}