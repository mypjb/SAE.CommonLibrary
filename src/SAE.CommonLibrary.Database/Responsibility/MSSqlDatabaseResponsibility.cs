using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Database.Responsibility
{
    /// <summary>
    /// 微软sqlserver<see cref="DatabaseResponsibility"/>实现
    /// </summary>
    /// <inheritdoc/>
    public class MSSqlDatabaseResponsibility : DatabaseResponsibility
    {
        /// <summary>
        /// 实例化一个mssql职责处理对象
        /// </summary>
        public MSSqlDatabaseResponsibility() : base("MSSql")
        {

        }

        protected override Task HandleCoreAsync(DatabaseResponsibilityContext context)
        {
            context.SetInvoke((connStr, options) =>
            {
                return Task.FromResult<IDbConnection>(new SqlConnection(connStr));
            });
            return Task.CompletedTask;
        }
    }
}
