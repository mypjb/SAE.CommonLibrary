using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.AspNetCore.Authorization;
using SAE.CommonLibrary.AspNetCore.Authorization.Bitmap;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.AspNetCore.Test.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class ABACAccountController : Controller
    {
        public ABACAccountController()
        {
        }

        public IActionResult Login()
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtClaimTypes.Name, "pjb"));
            identity.AddClaim(new Claim(JwtClaimTypes.BirthDate, "1994-02-27 00:00:00"));
            identity.AddClaim(new Claim(JwtClaimTypes.Subject, Guid.Empty.ToString("N")));
            identity.AddClaim(new Claim(JwtClaimTypes.Role, "admin"));
            identity.AddClaim(new Claim(JwtClaimTypes.Address, "127.0.0.1"));
            identity.AddClaim(new Claim(JwtClaimTypes.Gender, "man"));
            identity.AddClaim(new Claim(JwtClaimTypes.Scope, "api"));
            var claimsPrincipal = new ClaimsPrincipal(identity);
            return this.SignIn(claimsPrincipal, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
