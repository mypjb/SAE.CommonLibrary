using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SAE.CommonLibrary.Extension;
using System.Linq;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    public class BitmapAuthorizationHandler : AuthorizationHandler<BitmapAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBitmapEndpointStorage _bitmapEndpointStorage;

        public BitmapAuthorizationHandler(IHttpContextAccessor httpContextAccessor, IBitmapEndpointStorage bitmapEndpointStorage)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._bitmapEndpointStorage = bitmapEndpointStorage;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BitmapAuthorizationRequirement requirement)
        {
            var index = this._bitmapEndpointStorage.GetIndex(this._httpContextAccessor.HttpContext);

            if (index != -1)
            {
                var claim = context.User.FindFirst(Constant.PermissionBits);
                if (claim != null && claim.Value.IsNotNullOrWhiteSpace())
                {
                    var bits = claim.Value;

                    var bitIndex = index / Constant.PermissionBitsMaxPow;

                    if (bits.Count() >= bitIndex)
                    {
                        var bit = Encoding.ASCII.GetBytes(bits[bitIndex].ToString()).First();

                        var bitPosition = 1 << (index % Constant.PermissionBitsMaxPow);//将1向前推进 index % Constant.PermissionBitsMaxPow 个位

                        if ((bit | bitPosition) == bit)//bit位没有变化说明匹配权限位匹配正确
                        {
                            context.Succeed(requirement);
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
