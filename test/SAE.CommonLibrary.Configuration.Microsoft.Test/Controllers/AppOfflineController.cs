using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Configuration.Microsoft.Test.Controllers
{
    [ApiController]
    [Route("app/offlineconfig")]
    public class AppOfflineController : AppController
    {
        public AppOfflineController(OfflineOptions options) : base(options)
        {
        }

        [HttpGet]
        public override object Config(int version)
        {
            var options= this._options as OfflineOptions;
            if (!options.Init)
            {
                options.Init = true;
                return this.StatusCode((int)HttpStatusCode.NotFound);
            }
            return base.Config(version);
        }

        [HttpPost]
        public override Task<IActionResult> Change()
        {
            return base.Change();
        }

    }
}
