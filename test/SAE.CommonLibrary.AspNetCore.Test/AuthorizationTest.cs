using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.AspNetCore.Authorization;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Test;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.AspNetCore.Test
{
    public class AuthorizationTest : BaseTest
    {
        private readonly IBitmapAuthorization _authorization;

        public AuthorizationTest(ITestOutputHelper output) : base(output)
        {
            this._authorization = this._serviceProvider.GetService<IBitmapAuthorization>();
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddBitmapAuthorization();
            base.ConfigureServices(services);
        }

        [Fact]
        public void GeneratePermissionCode()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var max = 512;
            var permissionBits = Enumerable.Range(0, 16)
                                          .Select(s =>
                                          {
                                              var bit = Math.Abs(this.GetRandom().GetHashCode() % max);
                                              return bit > 0 ? bit : 1;
                                          })
                                          .ToArray();
            
            var code = this._authorization.GeneratePermissionCode(permissionBits);
            
            this.WriteLine(code);
            this.WriteLine(code.Length);
            this.WriteLine(permissionBits);
            permissionBits.ForEach(bit =>
            {
                Xunit.Assert.True(this._authorization.Authorizate(code, bit));
            });
            stopwatch.Stop();
            this.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
            while (true)
            {
                var bit = Math.Abs(this.GetRandom().GetHashCode() % max);
                if (permissionBits.Contains(bit)) continue;

                Xunit.Assert.False(this._authorization.Authorizate(code, bit));
                break;
            }
            
        }
    }
}
