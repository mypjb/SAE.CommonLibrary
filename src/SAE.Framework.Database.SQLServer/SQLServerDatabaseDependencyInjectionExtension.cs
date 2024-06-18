using SAE.Framework;
using SAE.Framework.Database.SQLServer.Responsibility;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="SAE.Framework.Constants.Database.Provider.SQLServer"/>数据库依赖注入配置
    /// </summary>
    public static class SQLServerDatabaseDependencyInjectionExtension
    {
        /// <summary>
        /// 添加<see cref="SAE.Framework.Constants.Database.Provider.SQLServer"/>数据库驱动
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <returns><paramref name="builder"/></returns>
        public static ISAEFrameworkBuilder AddSQLServerDatabase(this ISAEFrameworkBuilder builder)
        {
            return builder.AddDatabase<SQLServerDatabaseResponsibility>();
        }

    }
}
