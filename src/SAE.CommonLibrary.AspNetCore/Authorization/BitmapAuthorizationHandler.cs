using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Authorization
{
    public class BitmapAuthorizationHandler : AuthorizationHandler<BitmapAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BitmapAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, BitmapAuthorizationRequirement requirement)
        {
            var httpContext = this._httpContextAccessor.HttpContext;
            if (httpContext == null) return;
            var endpoint= httpContext.GetEndpoint();

            var path = string.Empty;

            if(endpoint is RouteEndpoint)
            {
                var routeEndpoint = (RouteEndpoint)endpoint;
                path = routeEndpoint.RoutePattern.RawText;
                foreach (var kv in routeEndpoint.RoutePattern.RequiredValues)
                {
                    path = path.Replace($"{{{kv.Key}}}", kv.Value?.ToString(), StringComparison.OrdinalIgnoreCase)
                               .ToLower();
                }
            }
            else
            {
                path = endpoint.DisplayName;
            }

            context.Succeed(requirement);
        }
    }
}
