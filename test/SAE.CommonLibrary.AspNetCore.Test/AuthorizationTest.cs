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
        public void Auth()
        {
            Enumerable.Range(0, 1000)
                      .AsParallel()
                      .ForEach(s =>
                      {
                          this.GenerateCode();
                      });
        }

        [Fact]
        public void GenerateCode()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var max = 4096;
            var permissionBits = Enumerable.Range(0, 100)
                                           .Select(s =>
                                          {
                                              var bit = Math.Abs(this.GetRandom().GetHashCode() % max);
                                              return bit == 0 ? 1 : bit;
                                          }).Distinct()
                                          .OrderBy(s => s)
                                          .ToArray();

            var code = this._authorization.GenerateCode(permissionBits);

            this.WriteLine(code);
            this.WriteLine(code.Length);
            this.WriteLine(permissionBits);
            permissionBits.ForEach(bit =>
            {
                var result = this._authorization.Authorize(code, bit);
                if (!result)
                {
                    this.WriteLine($"授权失败:{bit}");
                }
                Xunit.Assert.True(result);
            });
            stopwatch.Stop();
            this.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
            while (true)
            {
                var bit = Math.Abs(this.GetRandom().GetHashCode() % permissionBits.Max());
                bit = bit == 0 ? 1 : bit;
                if (permissionBits.Contains(bit)) continue;

                var result = this._authorization.Authorize(code, bit);

                this.WriteLine(bit);

                Xunit.Assert.False(result);
                break;
            }

        }
    }
}
