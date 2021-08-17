using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Database;
using SAE.CommonLibrary.EventStore.Document.Memory.Test;
using SAE.CommonLibrary.EventStore.Document.Memory.Test.Domain;
using SAE.CommonLibrary.EventStore.Document.MySql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.EventStore.Document.Test
{
    public class MySqlTest : MemoryTest
    {
        public MySqlTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddMySqlDocument();
        }


    }
}
