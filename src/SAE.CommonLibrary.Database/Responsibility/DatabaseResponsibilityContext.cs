using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SAE.CommonLibrary.Database.Responsibility
{
    public class DatabaseResponsibilityContext : Abstract.Responsibility.ResponsibilityContext
    {
        public DatabaseResponsibilityContext(DBConnectOptions options)
        {
            Options = options;
        }
        public DBConnectOptions Options { get; }

        public IDbConnection DbConnection { get; private set; }

        public void SetDbConnection(IDbConnection dbConnection)
        {
            this.DbConnection = dbConnection;
            this.Success();
        }
    }
}
