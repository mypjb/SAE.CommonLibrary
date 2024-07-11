using SAE.Framework;
using System.Data;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <c><see cref="Constants.Database.Provider.MySQL"/></c>数据库依赖注入配置
    /// </summary>
    public static class MySQLDatabaseDependencyInjectionExtension
    {
        /// <summary>
        /// 添加<c><see cref="Constants.Database.Provider.MySQL"/></c>数据库驱动
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <returns><paramref name="builder"/></returns>
        public static ISAEFrameworkBuilder AddMySQLDatabase(this ISAEFrameworkBuilder builder)
        {
            return builder.AddDatabase(Constants.Database.Provider.MySQL, (connStr, options) =>
            {
                return Task.FromResult<IDbConnection>(new MySqlConnector.MySqlConnection(connStr));
            });
        }

    }
}
