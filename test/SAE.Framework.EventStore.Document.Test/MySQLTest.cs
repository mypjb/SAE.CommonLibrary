using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit.Abstractions;

namespace SAE.Framework.EventStore.Document.Test
{
    public class MySQLTest : MemoryTest
    {
        public MySQLTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            this.range = new Random().Next(50, 100);
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSAEFramework()
                    .AddMySQLDocument();
        }
    }
}
