using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace SAE.Framework.Configuration.Microsoft.Test.Controllers
{
    [ApiController]
    [Route("{controller}/config")]
    public class AppController : Controller
    {
        protected readonly Options _options;
        protected readonly T1Options _t1Options;
        protected readonly T2Options _t2Options;

        public AppController(Options options,
        T1Options t1Options,
        T2Options t2Options)
        {
            this._options = options;
            this._t1Options = t1Options;
            this._t2Options = t2Options;
        }
        [HttpGet]
        [HttpGet("t1")]
        [HttpGet("t2")]
        public virtual object Config(int version)
        {
            var options = this.Get();

            if (options.Version == version)
            {
                return this.StatusCode((int)HttpStatusCode.NotModified);
            }

            var nextUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.Path}?{nameof(version)}={this._options.Version}";
            this.Response.Headers.Add(Constants.DefaultNextRequestHeaderName, nextUrl);
            return this.Content(options.Data, "application/json");
        }
        [HttpPost]
        [HttpPost("t1")]
        [HttpPost("t2")]
        public virtual async Task<IActionResult> Change()
        {
            var options = this.Get();
            using (var memoryStream = new System.IO.MemoryStream())
            {
                await this.Request.Body.CopyToAsync(memoryStream);

                byte[] bytes = memoryStream.ToArray();

                var json = Encoding.UTF8.GetString(bytes);

                options.Data = json;

                options.Version += 1;
            }

            return this.Ok();
        }

        private Options Get()
        {
            if (this.Request.Path.Value.EndsWith("t1"))
            {
                return this._t1Options;
            }
            else if (this.Request.Path.Value.EndsWith("t2"))
            {
                return this._t2Options;
            }
            else
            {
                return this._options;
            }
        }

    }
}
