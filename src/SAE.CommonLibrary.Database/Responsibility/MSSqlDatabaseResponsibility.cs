using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Database.Responsibility
{
    public class MSSqlDatabaseResponsibility : DatabaseResponsibility
    {
        public MSSqlDatabaseResponsibility() : base("MSSql")
        {
            
        }

        protected override Task HandleCoreAsync(DatabaseResponsibilityContext context)
        {
            context.SetDbConnection(new SqlConnection(context.Options.ConnectionString));
            return Task.CompletedTask;
        }
    }
}
