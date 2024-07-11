using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace SAE.Framework.Data.Test
{
    public class MemoryStorageTest : MongoDBStorageTest
    {
        public MemoryStorageTest(ITestOutputHelper output) : base(output)
        {
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSAEFramework()
                    .AddMemoryStorage();
        }
    }
}
