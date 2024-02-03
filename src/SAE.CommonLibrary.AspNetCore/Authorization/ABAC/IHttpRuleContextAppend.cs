using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SAE.CommonLibrary.Abstract.Authorization.ABAC;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    /// <summary>
    /// HttpRuleContext<seealso cref="SAE.CommonLibrary.Abstract.Authorization.ABAC.RuleContext"/>上下文附加接口
    /// </summary>
    public interface IHttpRuleContextAppend
    {
        /// <summary>
        /// 以字典的方式返回附加上下文
        /// </summary>
        /// <param name="ctx">请求上下文</param>
        Task<IDictionary<string,string>> GetContextAsync(HttpContext ctx);
    }
}