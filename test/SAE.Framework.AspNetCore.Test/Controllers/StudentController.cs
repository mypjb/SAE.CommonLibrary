using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SAE.Framework.Test;

namespace SAE.Framework.AspNetCore.Test.Controllers
{

    [Route("/noauth/api/[controller]")]
    public class StudentController : Controller
    {
        [HttpGet("[action]/{id}")]
        public object Display(string id)
        {
            return new { value = $"{nameof(StudentController)}_{nameof(Display)}" };
        }
        [HttpPost]
        public object Add()
        {
            return new { value = $"{nameof(StudentController)}_{nameof(Add)}" };
        }

        [HttpPut]
        public object Edit()
        {
            return new { value = $"{nameof(StudentController)}_{nameof(Edit)}" };
        }
    }
}
