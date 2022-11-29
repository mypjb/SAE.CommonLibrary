using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Scope.Test
{
    public class AspNetCoreHeaderScopeTest : AspNetCoreDomainScopeTest
    {

        public AspNetCoreHeaderScopeTest(ITestOutputHelper output) : base(output)
        {
        }
        
        protected override void ConfigureEnvironment(IHostBuilder builder, string environmentName = "Development")
        {
            builder.UseEnvironment("header");
        }
        [Fact]
        public override Task Get()
        {
            return base.Get();
        }

        protected override async Task<HttpRequestMessage> GetRrequestAsync(HttpMethod method, string pathString, string scopeName)
        {

            var req = new HttpRequestMessage(method, pathString);

            req.Headers.Add(this._options.Value.HeaderName,scopeName);

            return req;
        }
    }
}