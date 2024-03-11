using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Abstract.Authorization.ABAC;
using SAE.CommonLibrary.Caching;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{

    /// <summary>
    /// <see cref="IAuthorizeService"/>AspNetCore实现
    /// </summary>
    /// <inheritdoc/>
    public class AspNetCoreAuthorizeService : ConfigurationAuthorizeService<AspNetCoreAuthDescriptor>, IAuthorizeService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="authorizationPoliciesOptionsMonitor">授权策略配置监控器</param>
        /// <param name="authDescriptorsOptionsMonitor">授权描述配置监控器</param>
        /// <param name="memoryCache">本地内存</param>
        /// <param name="ruleContextFactory">规则上下文工厂</param>
        /// <param name="ruleDecoratorBuilder">规则装饰器构造器</param>
        /// <param name="logging">日志记录器</param>
        /// <param name="httpContextAccessor">http上下文连接器</param>
        public AspNetCoreAuthorizeService(IOptionsMonitor<List<AuthorizationPolicy>> authorizationPoliciesOptionsMonitor,
                                          IOptionsMonitor<List<AspNetCoreAuthDescriptor>> authDescriptorsOptionsMonitor,
                                          IMemoryCache memoryCache,
                                          IRuleContextFactory ruleContextFactory,
                                          IRuleDecoratorBuilder ruleDecoratorBuilder,
                                          ILogging<AspNetCoreAuthorizeService> logging,
                                          IHttpContextAccessor httpContextAccessor) : base(authorizationPoliciesOptionsMonitor,
                                                                                           authDescriptorsOptionsMonitor,
                                                                                           memoryCache,
                                                                                           ruleContextFactory,
                                                                                           ruleDecoratorBuilder,
                                                                                           logging)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc/>
        protected override Task<string> GetAuthDescriptorKeyAsync()
        {
            var ctx = this._httpContextAccessor.HttpContext;

            string path = ctx.Request.Path == Constants.Request.EndingSymbol.ToString() ?
                          ctx.Request.Path :
                          ctx.Request.Path.ToString().TrimEnd(Constants.Request.EndingSymbol);

            var key = $"{ctx.Request.Method}:{path}".ToLower();

            return Task.FromResult(key);
        }
        /// <inheritdoc/>
        protected override Task<bool> DefaultAuthAsync(RuleContext context)
        {
            this._logging.Info("使用默认授权规则。");
            return Task.FromResult(this._httpContextAccessor?.HttpContext.User.Identity?.IsAuthenticated ?? false);
        }
    }
}