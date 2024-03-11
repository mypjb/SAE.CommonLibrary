using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using SAE.CommonLibrary.Abstract.Authorization.ABAC;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    /// <summary>
    /// 基于用户的<see cref="IHttpRuleContextAppend"/>实现
    /// </summary>
    public class UserHttpRuleContextAppend : IHttpRuleContextAppend
    {
        private readonly ILogging _logging;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logging">日志记录器</param>
        public UserHttpRuleContextAppend(ILogging<UserHttpRuleContextAppend> logging)
        {
            this._logging = logging;
        }
        /// <inheritdoc/>
        public Task<IDictionary<string, string>> GetContextAsync(HttpContext ctx)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            if (ctx.User.Identity.IsAuthenticated && ctx.User.Claims.Any())
            {
                var url = ctx.Request.GetDisplayUrl();
                this._logging.Debug($"{url},用户认证成功准备附加用户属性");
                //用户只有通过认证才会将自身属性附着在上下文中，并且会顶替同名参数
                var claims = ctx.User.Claims;
                foreach (var claim in claims)
                {
                    var key = $"{Constants.ABAC.User}.{claim.Type.ToLower()}";
                    dict[key] = claim.Value;
                }
            }
            return Task.FromResult(dict);
        }
    }
}