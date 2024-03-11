using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using SAE.CommonLibrary.Abstract.Authorization.ABAC;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    ///<inheritdoc cref="IRuleContextProvider"/>
    /// <summary>
    /// 获得基于Http的<code>ABAC</code>上下文对象<see cref="RuleContext"/>
    /// </summary> 
    public class HttpRuleContextProvider : IRuleContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogging _logging;
        private readonly IEnumerable<IHttpRuleContextAppend> _httpRuleContextAppends;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="httpContextAccessor">httpContext连接器</param>
        /// <param name="logging">日至记录器</param>
        /// <param name="httpRuleContextAppends"><see cref="IHttpRuleContextAppend"/>附加上下文</param>
        public HttpRuleContextProvider(IHttpContextAccessor httpContextAccessor,
                                       ILogging<HttpRuleContextProvider> logging,
                                       IEnumerable<IHttpRuleContextAppend> httpRuleContextAppends)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._logging = logging;
            this._httpRuleContextAppends = httpRuleContextAppends;
        }
        /// <inheritdoc/>
        public async Task<RuleContext> GetAsync()
        {

            var ctx = this._httpContextAccessor.HttpContext;

            var url = ctx.Request.GetDisplayUrl();

            this._logging.Info($"准备获取ABAC认证上下文：{url}");

            var ruleContext = new RuleContext(new Dictionary<string, string>());

            foreach (var httpRuleContextAppend in _httpRuleContextAppends)
            {
                ruleContext.Merge(await httpRuleContextAppend.GetContextAsync(ctx));
            }

            this._logging.Debug($"{url}，ABAC认证上下文:{ruleContext}");

            return ruleContext;
        }
    }
}