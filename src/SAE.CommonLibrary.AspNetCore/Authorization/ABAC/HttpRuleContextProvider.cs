using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Hosting;
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
        private readonly IHostEnvironment _environment;
        private readonly ILogging _logging;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="environment"></param>
        /// <param name="logging"></param>
        public HttpRuleContextProvider(IHttpContextAccessor httpContextAccessor,
                                       IHostEnvironment environment,
                                       ILogging<HttpRuleContextProvider> logging)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._environment = environment;
            this._logging = logging;
        }

        public Task<RuleContext> GetAsync()
        {

            var ctx = this._httpContextAccessor.HttpContext;

            var url = ctx.Request.GetDisplayUrl();
            this._logging.Info($"准备获取ABAC认证上下文：{url}");
            
            var dict = new Dictionary<string, string>();

            dict[Constants.ABAC.Authentication] = ctx.User.Identity.IsAuthenticated.ToString();

            dict[Constants.ABAC.Path] = ctx.Request.Path;

            dict[Constants.ABAC.AppName] = this._environment.ApplicationName;

            dict[Constants.ABAC.Environment] = _environment.EnvironmentName;

            dict[Constants.ABAC.Timestamp] = Utils.Timestamp().ToString();

            dict[Constants.ABAC.ClientIP] = ctx.Request.GetClientIP();

            dict[Constants.ABAC.ServerIP] = Utils.Network.GetServerIP();

            dict[Constants.ABAC.Scheme] = ctx.Request.Scheme;

            dict[Constants.ABAC.Host] = ctx.Request.Host.Host;

            dict[Constants.ABAC.Port] = ctx.Request.Host.Port.ToString();

            if (ctx.User.Identity.IsAuthenticated && ctx.User.Claims.Any())
            {
                this._logging.Debug($"{url},用户认证成功准备附加用户属性,当前属性:{dict.ToJsonString()}");
                //用户只有通过认证才会将自身属性附着在上下文中，并且会顶替同名参数
                var claims = ctx.User.Claims;
                foreach (var claim in claims)
                {
                    dict[claim.Type.ToLower()] = claim.Value;
                }
            }

            var ruleContext = new RuleContext(dict);

            this._logging.Debug($"{url}，ABAC认证上下文:{ruleContext}");

            return Task.FromResult(ruleContext);
        }
    }
}