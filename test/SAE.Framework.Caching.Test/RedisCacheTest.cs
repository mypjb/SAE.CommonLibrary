using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace SAE.Framework.Cache.Test
{
    public  class RedisCacheTest : MemoryCacheTest
    {

        public RedisCacheTest(ITestOutputHelper output) : base(output)
        {
           
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSAEFramework()
                    .AddRedisCache();
        }

       
    }
}
