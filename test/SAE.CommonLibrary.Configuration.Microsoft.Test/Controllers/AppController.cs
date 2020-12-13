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
    [Route("{controller}/config")]
    public class AppController : Controller
    {
        private readonly Options _options;

        public AppController(Options options)
        {
            this._options = options;
        }
        [HttpGet]
        public object Config(int version)
        {
            if (this._options.Version == version)
            {
                return this.StatusCode((int)HttpStatusCode.NotModified);
            }

            var nextUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.Path}?{nameof(version)}={this._options.Version}";
            this.Response.Headers.Add(SAEConfigurationProvider.ConfigUrl, nextUrl);
            return this.Content(this._options.Data, "application/json");
        }
        [HttpPost]
        public async Task<IActionResult> Change()
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                await this.Request.Body.CopyToAsync(memoryStream);

                byte[] bytes = memoryStream.ToArray();

                var json = Encoding.UTF8.GetString(bytes);

                this._options.Data = json;

                this._options.Version += 1;
            }

            return this.Ok();
        }

    }
}
