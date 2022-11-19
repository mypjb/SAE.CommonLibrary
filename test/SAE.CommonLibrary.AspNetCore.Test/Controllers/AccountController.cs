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

namespace SAE.CommonLibrary.AspNetCore.Test.Controllers
{
    [Route("[controller]/[action]")]

    public class AccountController : Controller
    {
        private readonly IBitmapEndpointStorage _bitmapEndpointStorage;
        private readonly IBitmapAuthorization _bitmapAuthorization;
        private readonly IOptionsSnapshot<List<BitmapAuthorizationDescriptor>> _options;

        public AccountController(IBitmapEndpointStorage bitmapEndpointStorage,
                                 IBitmapAuthorization bitmapAuthorization,
                                 IOptionsSnapshot<List<BitmapAuthorizationDescriptor>> options)
        {
            this._bitmapEndpointStorage = bitmapEndpointStorage;
            this._bitmapAuthorization = bitmapAuthorization;
            this._options = options;
        }
        [AllowAnonymous]
        public IActionResult Login(string path, string method)
        {
            var index = this._bitmapEndpointStorage.GetIndex(path, method);

            var code = this._bitmapAuthorization.GenerateCode(new[] { index });

            var descriptors = this._options.Value;

            var descriptor = descriptors.FirstOrDefault(s => this._bitmapAuthorization.Authorize(s.Code, index));

            if (descriptor == null)
            {
                descriptor = new BitmapAuthorizationDescriptor
                {
                    Index = descriptors.Count + 1,
                    Name = $"role_{descriptors.Count}",
                    Description = "this is role",
                    Code = code
                };
                descriptors.Add(descriptor);
            }

            var bitmapCode = this._bitmapAuthorization.GenerateCode(new[] { descriptor.Index });

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.Name, "pjb"));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.Empty.ToString("N")));
            identity.AddClaim(new Claim(Constants.BitmapAuthorize.Claim, bitmapCode));

            var claimsPrincipal = new ClaimsPrincipal(identity);

            return SignIn(claimsPrincipal, CookieAuthenticationDefaults.AuthenticationScheme);
        }

    }
}
