using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SAE.Framework.Extension;

namespace SAE.Framework.Scope.Test.Controllers
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

        [HttpPost("{action}/{name}")]
        public async Task<object> Switch(string name)
        {
            var switchName = string.Empty;
            var current = await this._scopeFactory.GetAsync();

            using (var scope = await this._scopeFactory.GetAsync(name))
            {
                Assert.Build(current.Name.Equals(scope.Name))
                      .False();
                Assert.Build(name.Equals(scope.Name))
                      .True();
                switchName = scope.Name;
            }

            var newScope = await this._scopeFactory.GetAsync();

            Assert.Build(current.Name.Equals(newScope.Name))
                  .True();

            return switchName;
        }
    }
}