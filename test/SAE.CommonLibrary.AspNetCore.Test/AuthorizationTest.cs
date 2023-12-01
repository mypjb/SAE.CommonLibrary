using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.AspNetCore.Authorization;
using SAE.CommonLibrary.AspNetCore.Authorization.Bitmap;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Test;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.AspNetCore.Test
{
    public class AuthorizationTest : BaseTest
    {
        private readonly IAuthorization _authorization;

        public AuthorizationTest(ITestOutputHelper output) : base(output)
        {
            this._authorization = this._serviceProvider.GetService<IAuthorization>();
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IAuthorization, DefaultAuthorization>();
            services.AddDefaultScope()
                    .AddNlogLogger();
            base.ConfigureServices(services);
        }
        [Fact]
        public void Auth()
        {
            var random = new Random();
            Enumerable.Range(0, random.Next(66))
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
            var max = 3;
            var random = new Random();
            var permissionBits = Enumerable.Range(0, random.Next(6666))
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
            permissionBits.AsParallel().ForAll(bit =>
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

            var i = 0;

            var num = random.Next(permissionBits.Max() / 3);

            while (true)
            {
                i++;
                if (permissionBits.Contains(i)) continue;

                num--;

                var result = this._authorization.Authorize(code, i);

                this.WriteLine(i);

                Xunit.Assert.False(result);
                if (num < 0)
                {
                    break;
                }
            }

        }
    }
}
