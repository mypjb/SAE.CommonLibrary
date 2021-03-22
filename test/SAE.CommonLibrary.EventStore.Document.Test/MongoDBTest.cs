using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.EventStore.Document.Memory.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.EventStore.Document.Test
{
    public class MongoDBTest : MemoryTest
    {
        public MongoDBTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        protected override void ConfigureServicesBefore(IServiceCollection services)
        {
            services.AddMongoDB();
            base.ConfigureServicesBefore(services);
        }
    }
}
