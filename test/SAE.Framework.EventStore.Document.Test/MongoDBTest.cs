using Microsoft.Extensions.DependencyInjection;
using SAE.Framework.EventStore.Document.Memory.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace SAE.Framework.EventStore.Document.Test
{
    public class MongoDBTest : MemoryTest
    {
        public MongoDBTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            this.range = new Random().Next(50, 100);
        }

        protected override void ConfigureServicesBefore(IServiceCollection services)
        {
            services.AddSAEFramework()
                    .AddMongoDB();
            base.ConfigureServicesBefore(services);
        }
    }
}
