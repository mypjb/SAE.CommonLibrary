using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Scope;

namespace SAE.CommonLibrary.Configuration.Microsoft.Test.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class MulitTenanController : Controller
    {
        private readonly IOptions<Options> _options;
        private readonly IOptionsSnapshot<Options> _optionsSnapshot;
        private readonly IOptionsMonitor<Options> _optionsMonitor;
        private readonly IScopeFactory _scopeFactory;

        public MulitTenanController(IOptions<Options> options,
                                    IOptionsSnapshot<Options> optionsSnapshot,
                                    IOptionsMonitor<Options> optionsMonitor,
                                    IScopeFactory scopeFactory)
        {
            this._options = options;
            this._optionsSnapshot = optionsSnapshot;
            this._optionsMonitor = optionsMonitor;
            this._scopeFactory = scopeFactory;
        }

        [HttpGet]
        public async Task<object> Get([FromQuery] string name)
        {
            using (var scope = await this._scopeFactory.GetAsync(name))
            {
                return new[] { this._options.Value, this._optionsSnapshot.Value, this._optionsMonitor.CurrentValue };
            }
        }

    }
}