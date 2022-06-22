using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.Scope.Test.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class HomeController : Controller
    {
        private readonly IScopeFactory _scopeFactory;

        public HomeController(IScopeFactory scopeFactory)
        {
            this._scopeFactory = scopeFactory;
        }
        [HttpGet]
        public async Task<object> Get()
        {
            var scope = await this._scopeFactory.GetAsync();

            return scope.Name;
        }

        [HttpPost("{action}/name")]
        public async Task<ActionResult> Switch(string name)
        {
            var current = await this._scopeFactory.GetAsync();

            using (var scope =await this._scopeFactory.GetAsync(name))
            {
                Assert.Build(current.Name.Equals(scope.Name))
                      .True();
                Assert.Build(name.Equals(scope.Name))
                      .True();
            }

            var newScope = await this._scopeFactory.GetAsync();

            Assert.Build(current.Name.Equals(newScope.Name))
                  .True();

            return this.Ok();
        }
    }
}