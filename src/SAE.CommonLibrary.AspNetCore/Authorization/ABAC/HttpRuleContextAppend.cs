using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    public class HttpRuleContextAppend : IHttpRuleContextAppend
    {
        private readonly IHostEnvironment _environment;
        private readonly ILogging _logging;
        public HttpRuleContextAppend(IHostEnvironment environment,
                                     ILogging<HttpRuleContextAppend> logging)
        {
            _environment = environment;
            _logging = logging;
        }
        public Task<IDictionary<string, string>> GetContextAsync(HttpContext ctx)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();

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

            return Task.FromResult(dict);
        }
    }
}