using SAE.CommonLibrary.Database.SQLServer.Responsibility;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="SAE.CommonLibrary.Constants.Database.Provider.SQLServer"/>数据库依赖注入配置
    /// </summary>
    public static class SQLServerDatabaseDependencyInjectionExtension
    {
        /// <summary>
        /// 添加<see cref="SAE.CommonLibrary.Constants.Database.Provider.SQLServer"/>数据库驱动
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns><paramref name="services"/></returns>
        public static IServiceCollection AddSQLServerDatabase(this IServiceCollection services)
        {
            return services.AddDatabase<SQLServerDatabaseResponsibility>();
        }

    }
}
