using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.AspNetCore.Test.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class HomeController:Controller
    {
        [HttpGet("{action}")]
        public  object Index()
        {
            return new { value = $"{nameof(HomeController)}_{nameof(Index)}" };
        }
        [HttpPost("{action}")]
        public object Add()
        {
            return new { value = $"{nameof(HomeController)}_{nameof(Add)}" };
        }
        [HttpPut("{action}")]
        public object Edit()
        {
            return new { value = $"{nameof(HomeController)}_{nameof(Edit)}" };
        }

        [HttpDelete("{action}")]
        public object Delete()
        {
            return new { value = $"{nameof(HomeController)}_{nameof(Delete)}" };
        }
        [HttpGet("~/{controller}/{action}")]
        public object Custom()
        {
            return new { value = $"{nameof(HomeController)}_{nameof(Custom)}" };
        }
    }
}
