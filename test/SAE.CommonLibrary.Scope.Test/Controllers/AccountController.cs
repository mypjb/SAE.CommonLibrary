using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Scope.AspNetCore;

namespace SAE.CommonLibrary.Scope.Test.Controllers
{
    [Route("{controller}/{action}")]

    public class AccountController : Controller
    {
        private readonly MultiTenantOptions _options;

        public AccountController(IOptions<MultiTenantOptions> options)
        {
            this._options = options.Value;
        }
        [AllowAnonymous]
        public IActionResult Login([FromQuery]string name)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.Name, Guid.NewGuid().ToString("N")));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.Empty.ToString("N")));
            identity.AddClaim(new Claim(this._options.ClaimName, name));

            var claimsPrincipal = new ClaimsPrincipal(identity);

            return SignIn(claimsPrincipal, CookieAuthenticationDefaults.AuthenticationScheme);
        }

    }
}
