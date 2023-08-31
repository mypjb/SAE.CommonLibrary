using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC
{
    public interface IAuthorizeService
    {
        /// <summary>
        /// 
        /// </summary>
        Task<bool> AuthAsync(AuthorizeContext context);
    }
}