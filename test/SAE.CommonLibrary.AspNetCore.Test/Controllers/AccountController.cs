using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Test.Controllers
{
    [Route("{controller}/{action}")]

    public class AccountController:Controller
    {
        private readonly IBitmapEndpointStorage _bitmapEndpointStorage;
        private readonly IBitmapAuthorization _bitmapAuthorization;
        private readonly IOptions<SystemOptions> _options;

        public AccountController(IBitmapEndpointStorage bitmapEndpointStorage,
                                 IBitmapAuthorization bitmapAuthorization,
                                 IOptions<SystemOptions> options)
        {
            this._bitmapEndpointStorage = bitmapEndpointStorage;
            this._bitmapAuthorization = bitmapAuthorization;
            this._options = options;
        }
        [AllowAnonymous]
        public IActionResult Login(string path,string method)
        {
            var index = this._bitmapEndpointStorage.GetIndex(path, method);

            var code = this._bitmapAuthorization.GeneratePermissionCode(new[] { index });

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.Name, "pjb"));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.Empty.ToString("N")));
            identity.AddClaim(new Claim(Constants.BitmapAuthorize.Claim, string.Format(Constants.BitmapAuthorize.Format,this._options.Value.Id,code)));

            var claimsPrincipal = new ClaimsPrincipal(identity);

            return SignIn(claimsPrincipal, CookieAuthenticationDefaults.AuthenticationScheme);
        }

    }
}
