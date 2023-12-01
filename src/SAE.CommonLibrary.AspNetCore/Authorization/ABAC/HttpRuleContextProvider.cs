using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SAE.CommonLibrary.Abstract.Authorization.ABAC;
using SAE.CommonLibrary.Extension;

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="environment"></param>
        public HttpRuleContextProvider(IHttpContextAccessor httpContextAccessor,
                                                    IHostEnvironment environment)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._environment = environment;
        }

        public Task<RuleContext> GetAsync()
        {
            var ctx = this._httpContextAccessor.HttpContext;

            var claims = ctx.User.Claims;

            var dict = new Dictionary<string, string>();

            foreach (var claim in claims)
            {
                dict[claim.Type.ToLower()] = claim.Value;
            }

            dict[Constants.ABAC.Path] = ctx.Request.Path;

            dict[Constants.ABAC.AppName] = this._environment.ApplicationName;

            dict[Constants.ABAC.Environment] = _environment.EnvironmentName;

            dict[Constants.ABAC.Timestamp] = Utils.Timestamp().ToString();

            dict[Constants.ABAC.ClientIP] = ctx.Request.GetClientIP();

            dict[Constants.ABAC.ServerIP] = Utils.Network.GetServerIP();

            dict[Constants.ABAC.Scheme] = ctx.Request.Scheme;

            dict[Constants.ABAC.Host] = ctx.Request.Host.Host;

            dict[Constants.ABAC.Port] = ctx.Request.Host.Port.ToString();

            return Task.FromResult(new RuleContext(dict));
        }
    }
}