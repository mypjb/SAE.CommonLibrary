using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SAE.CommonLibrary.Abstract.Authorization.ABAC;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    /// <summary>
    /// ABAC授权认证的实现
    /// </summary>
    /// <inheritdoc/>
    public class AuthorizationHandler : AuthorizationHandler<ABACAuthorizationRequirement>
    {
        private readonly ILogging _logging;
        private readonly IAuthorizeService _authorizeService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logging">日志记录器</param>
        /// <param name="authorizeService">授权服务</param>
        public AuthorizationHandler(ILogging<AuthorizationHandler> logging,
                                    IAuthorizeService authorizeService)
        {
            this._logging = logging;
            this._authorizeService = authorizeService;
        }
        /// <inheritdoc/>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ABACAuthorizationRequirement requirement)
        {
            this._logging.Info("准备进入ABAC授权管道");
            if (await _authorizeService.AuthAsync())
            {
                //授权成功
                context.Succeed(requirement);
            }
        }
    }
}