using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    public interface IAuthorizationPolicy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        Task<IAuthorizationRequirement[]> GetAsync(AuthorizeContext context);
    }
}