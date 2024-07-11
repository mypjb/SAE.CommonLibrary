using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Xunit.Abstractions;

namespace SAE.Framework.Scope.Test
{
    public class AspNetCoreUserScopeTest : AspNetCoreDomainScopeTest
    {

        public AspNetCoreUserScopeTest(ITestOutputHelper output) : base(output)
        {
        }
        
        protected override void ConfigureEnvironment(IHostBuilder builder, string environmentName = "Development")
        {
            builder.UseEnvironment("user");
        }

        protected override async Task<HttpRequestMessage> GetRrequestAsync(HttpMethod method, string pathString, string scopeName)
        {

            var req = new HttpRequestMessage(method, pathString);

            var httpResponse = await this._client.GetAsync($"/account/login?name={scopeName}");

            var cookies = httpResponse.Headers.GetValues(HeaderNames.SetCookie);

            req.Headers.Add(HeaderNames.Cookie, cookies);

            return req;
        }
    }
}