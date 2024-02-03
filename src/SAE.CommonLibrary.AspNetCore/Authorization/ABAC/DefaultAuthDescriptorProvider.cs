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


        public AspNetCoreAuthorizeService(IOptionsMonitor<AuthorizationPolicy[]> authorizationPoliciesOptionsMonitor,
                                          IOptionsMonitor<AspNetCoreAuthDescriptor[]> authDescriptorsOptionsMonitor,
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

        protected override Task<string> GetAuthDescriptorKeyAsync()
        {
            var ctx = this._httpContextAccessor.HttpContext;

            string path = ctx.Request.Path == Constants.Request.EndingSymbol.ToString() ?
                          ctx.Request.Path :
                          ctx.Request.Path.ToString().TrimEnd(Constants.Request.EndingSymbol);

            var key = $"{ctx.Request.Method}:{path}".ToLower();

            return Task.FromResult(key);
        }
        protected override Task<bool> DefaultAuthAsync(RuleContext context)
        {
            this._logging.Info("使用默认授权规则。");
            return Task.FromResult(this._httpContextAccessor?.HttpContext.User.Identity?.IsAuthenticated ?? false);
        }
    }
}