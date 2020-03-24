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
        public object Display(Guid id)
        {
            return new { value = $"{nameof(StudentController)}_{nameof(Display)}" };
        }
        [HttpPost]
        public object Add([FromBody]Student student)
        {
            return new { value = $"{nameof(StudentController)}_{nameof(Add)}" };
        }

        [HttpPut]
        public object Edit([FromBody]Student student)
        {
            return new { value = $"{nameof(StudentController)}_{nameof(Edit)}" };
        }
    }
}
