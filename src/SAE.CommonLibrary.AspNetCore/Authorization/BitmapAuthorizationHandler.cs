using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SAE.CommonLibrary.Extension;
using System.Linq;
using System.Security.Claims;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    public class BitmapAuthorizationHandler : AuthorizationHandler<BitmapAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBitmapEndpointStorage _bitmapEndpointStorage;
        private readonly IBitmapAuthorization _bitmapAuthorization;

        public BitmapAuthorizationHandler(IHttpContextAccessor httpContextAccessor,
                                          IBitmapEndpointStorage bitmapEndpointStorage,
                                          IBitmapAuthorization bitmapAuthorization)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._bitmapEndpointStorage = bitmapEndpointStorage;
            this._bitmapAuthorization = bitmapAuthorization;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BitmapAuthorizationRequirement requirement)
        {
            if (!context.User.FindFirst(Constants.BitmapAuthorize.Administrator)?.Value.IsNullOrWhiteSpace() ?? false)//是否是超管
            {
                context.Succeed(requirement);
            }
            else
            {
                var index = this._bitmapEndpointStorage.GetIndex(this._httpContextAccessor.HttpContext);

                if (index > -1)
                {
                    var claims = context.User.FindAll(Constants.BitmapAuthorize.Claim) ?? Enumerable.Empty<Claim>();

                    var code = this._bitmapAuthorization.FindPermissionCode(claims);

                    if (!string.IsNullOrEmpty(code))
                    {
                        this._bitmapAuthorization.Authorizate(code, index);
                        context.Succeed(requirement);
                    }
                }
                else
                {
                    //index not exist default auth
                    context.Succeed(requirement);
                }
            }


            return Task.CompletedTask;
        }
    }
}
