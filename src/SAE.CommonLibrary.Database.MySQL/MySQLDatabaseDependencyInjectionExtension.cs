using SAE.CommonLibrary;
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
        /// <param name="services">服务集合</param>
        /// <returns><paramref name="services"/></returns>
        public static IServiceCollection AddMySQLDatabase(this IServiceCollection services)
        {
            return services.AddDatabase(Constants.Database.Provider.MySQL, (connStr, options) =>
            {
                return Task.FromResult<IDbConnection>(new MySqlConnector.MySqlConnection(connStr));
            });
        }

    }
}
