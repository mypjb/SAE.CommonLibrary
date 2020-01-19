using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Caching;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Cache.Redis.Test
{
    public  class RedisCacheTest : MemoryCacheTest
    {

        public RedisCacheTest(ITestOutputHelper output) : base(output)
        {
           
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddRedisCache();
        }

    }
}
