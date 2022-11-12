using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Database;
using SAE.CommonLibrary.Database.Responsibility;
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 数据库依赖注入扩展程序
    /// </summary>
    public static class DatabaseDependencyInjectionExtension
    {
        internal const string MYSQL = "mysql";
        /// <summary>
        /// 注册默认<see cref="DefaultDBConnectionFactory"/>
        /// </summary>
        /// <param name="services"></param>
        private static IServiceCollection AddDefaultDBConnectionFactory(this IServiceCollection services)
        {
            if (!services.IsRegister<IDBConnectionFactory>())
            {
                services.AddOptions<List<DBConnectOptions>>()
                        .Bind(DBConnectOptions.Option);

                services.TryAddSingleton<IDBConnectionFactory, DefaultDBConnectionFactory>();
            }

            return services;
        }


        /// <summary>
        /// 添加数据库驱动
        /// </summary>
        /// <param name="services"></param>
        /// <typeparam name="TDatabaseResponsibility">处理程序类</typeparam>
        public static IServiceCollection AddDatabase<TDatabaseResponsibility>(this IServiceCollection services) where TDatabaseResponsibility : DatabaseResponsibility
        {
            services.AddDefaultDBConnectionFactory()
                    .AddResponsibility<DatabaseResponsibilityContext, TDatabaseResponsibility>();
            return services;
        }

        /// <summary>
        /// 使用委托的方式，添加数据库驱动。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="provider">驱动名称</param>
        /// <param name="handler">处理函数</param>
        public static IServiceCollection AddDatabase(this IServiceCollection services,
            string provider,
            Func<string, DBConnectOptions, Task<IDbConnection>> handler)
        {
            services.AddDefaultDBConnectionFactory()
                    .AddResponsibility<DatabaseResponsibilityContext, DelegateDatabaseResponsibility>(new DelegateDatabaseResponsibility(provider, handler));
            return services;
        }

        /// <summary>
        /// 添加<c>mssql</c>数据库驱动
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddMSSqlDatabase(this IServiceCollection services)
        {
            return services.AddDatabase<MSSqlDatabaseResponsibility>();
        }
        /// <summary>
        /// 添加<c>mysql</c>数据库驱动
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddMySqlDatabase(this IServiceCollection services)
        {
            return services.AddDatabase(MYSQL, (connStr, options) =>
            {
                return Task.FromResult<IDbConnection>(new MySqlConnector.MySqlConnection(connStr));
            });
        }
    }
}
