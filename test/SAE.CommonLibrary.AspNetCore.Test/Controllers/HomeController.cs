using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.AspNetCore.Test.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/{controller}")]
    public class HomeController:Controller
    {
        [HttpGet]
        public  object Index()
        {
            return new { value = $"{nameof(HomeController)}_{nameof(Index)}" };
        }
        [HttpPost]
        public object Add()
        {
            return new { value = $"{nameof(HomeController)}_{nameof(Add)}" };
        }
        [HttpPut]
        public object Edit()
        {
            return new { value = $"{nameof(HomeController)}_{nameof(Add)}" };
        }

        [HttpDelete]
        public object Remove()
        {
            return new { value = $"{nameof(HomeController)}_{nameof(Remove)}" };
        }
        [HttpGet("~/{controller}/{action}")]
        public object Custom()
        {
            return new { value = $"{nameof(HomeController)}_{nameof(Remove)}" };
        }
    }
}
