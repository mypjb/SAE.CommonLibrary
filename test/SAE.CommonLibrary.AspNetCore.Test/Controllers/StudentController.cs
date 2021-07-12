using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAE.CommonLibrary.Test;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.AspNetCore.Test.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class StudentController : Controller
    {
        [HttpGet("{action}/{id}")]
        public object Display(string id)
        {
            return new { value = $"{nameof(StudentController)}_{nameof(Display)}" };
        }
        [HttpPost("{action}")]
        public object Add()
        {
            return new { value = $"{nameof(StudentController)}_{nameof(Add)}" };
        }

        [HttpPost("{action}")]
        public object Edit()
        {
            return new { value = $"{nameof(StudentController)}_{nameof(Edit)}" };
        }
    }
}
