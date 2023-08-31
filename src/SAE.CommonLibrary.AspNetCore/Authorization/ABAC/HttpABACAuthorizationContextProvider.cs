using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    /// <summary>
    /// 获得基于Http的<code>ABAC</code>上下文对象<see cref="ABACAuthorizationContext"/>
    /// </summary> 
    public class HttpABACAuthorizationContextProvider : IABACAuthorizationContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _environment;

        public HttpABACAuthorizationContextProvider(IHttpContextAccessor httpContextAccessor,
                                                    IHostEnvironment environment)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._environment = environment;
        }
        public async Task<ABACAuthorizationContext> GetAsync()
        {
            var ctx = this._httpContextAccessor.HttpContext;

            var claims = ctx.User.Claims;

            var authorizationContext = new ABACAuthorizationContext();

            foreach (var claim in claims)
            {
                authorizationContext.Add(claim.Type, claim.Value);
            }

            authorizationContext.Add(nameof(ctx.Request.Path), ctx.Request.Path);

            authorizationContext.Add(Constants.ABAC.Environment, _environment.EnvironmentName);

            authorizationContext.Add(Constants.ABAC.Timestamp, Utils.Timestamp().ToString());

            authorizationContext.Add(Constants.ABAC.ClientIP, "");

            authorizationContext.Add(Constants.ABAC.ServerIP, "");

            authorizationContext.Add(Constants.ABAC.Scheme, ctx.Request.Scheme);

            authorizationContext.Add(Constants.ABAC.Scheme, "");

            authorizationContext.Add(Constants.ABAC.Host, ctx.Request.Host.Host);

            authorizationContext.Add(Constants.ABAC.Port, ctx.Request.Host.Port.ToString());

            return authorizationContext;
        }
    }
}