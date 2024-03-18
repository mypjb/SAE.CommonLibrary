using Microsoft.Data.SqlClient;
using SAE.CommonLibrary.Database.Responsibility;
using System.Data;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Database.SQLServer.Responsibility
{
    /// <summary>
    /// 微软<see cref="SAE.CommonLibrary.Constants.Database.Provider.SQLServer"/><see cref="DatabaseResponsibility"/>实现
    /// </summary>
    /// <inheritdoc/>
    public class SQLServerDatabaseResponsibility : DatabaseResponsibility
    {
        /// <summary>
        /// 实例化一个mssql职责处理对象
        /// </summary>
        public SQLServerDatabaseResponsibility() : base(Constants.Database.Provider.SQLServer)
        {

        }
        /// <inheritdoc/>
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
