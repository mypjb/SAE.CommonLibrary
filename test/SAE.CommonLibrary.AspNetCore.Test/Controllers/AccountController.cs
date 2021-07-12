using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.AspNetCore.Test.Controllers
{
    [ApiController]
    public class AccountController:Controller
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            return this.Ok();
        }
    }
}
