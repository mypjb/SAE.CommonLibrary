using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Abstract.Builder;
using SAE.CommonLibrary.Database;
using SAE.CommonLibrary.EventStore.Document.Memory.Test;
using SAE.CommonLibrary.EventStore.Document.Memory.Test.Domain;
using SAE.CommonLibrary.EventStore.Document.MySql;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.EventStore.Document.Test
{
    public class MySqlTest : MemoryTest
    {
        private const string DataInfoName = "DataInfo";
        private const string SQLPath = "../../../../../EventStore.sql";
        public MySqlTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddMySqlDocument();
        }

        protected override void Configure(IServiceProvider provider)
        {
            base.Configure(provider);

            var dBConnectionFactory = provider.GetService<IDBConnectionFactory>();
            var eventStoreDBName = dBConnectionFactory.Get().Database;
            using (var conn = dBConnectionFactory.Get(DataInfoName))
            {
                var results = conn.Query<string>("SELECT SCHEMA_NAME FROM SCHEMATA where SCHEMA_NAME=@Database", new { Database = eventStoreDBName });
                if (results.Count() != 0)
                {
                    return;
                }

                conn.Execute(File.ReadAllText(SQLPath));
            }


        }
        [Fact]
        public override Task ChangePassword()
        {
            return base.ChangePassword();
        }

    }
}
