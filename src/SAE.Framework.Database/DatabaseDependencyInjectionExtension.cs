using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.Framework;
using SAE.Framework.Database;
using SAE.Framework.Database.Responsibility;
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 数据库依赖注入扩展程序
    /// </summary>
    public static class DatabaseDependencyInjectionExtension
    {

        /// <summary>
        /// 注册默认<see cref="DefaultDBConnectionFactory"/>
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <returns><paramref name="builder"/></returns>
        private static ISAEFrameworkBuilder AddDefaultDBConnectionFactory(this ISAEFrameworkBuilder builder)
        {
            var services = builder.Services;

            if (!services.IsRegister<IDBConnectionFactory>())
            {
                services.AddOptions<List<DBConnectOptions>>()
                        .Bind(DBConnectOptions.Option)
                        .PostConfigure<IDBConnectOptionsConfigure>((options, configure) =>
                        {
                            configure.Configure(options);
                        });

                services.TryAddSingleton<IDBConnectionFactory, DefaultDBConnectionFactory>();
                services.TryAddSingleton<IDBConnectOptionsConfigure, DefaultDBConnectOptionsConfigure>();
            }

            return builder;
        }


        /// <summary>
        /// 添加数据库驱动
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <typeparam name="TDatabaseResponsibility">处理程序类</typeparam>
        /// <returns><paramref name="builder"/></returns>
        public static ISAEFrameworkBuilder AddDatabase<TDatabaseResponsibility>(this ISAEFrameworkBuilder builder) where TDatabaseResponsibility : DatabaseResponsibility
        {
            var services = builder.Services;
            builder.AddDefaultDBConnectionFactory()
                   .AddResponsibility<DatabaseResponsibilityContext, TDatabaseResponsibility>();
            builder.AddDefaultLogger();
            return builder;
        }

        /// <summary>
        /// 使用委托的方式，添加数据库驱动。
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <param name="provider">驱动名称</param>
        /// <param name="handler">处理函数</param>
        /// <returns><paramref name="builder"/></returns>
        public static ISAEFrameworkBuilder AddDatabase(this ISAEFrameworkBuilder builder,
            string provider,
            Func<string, DBConnectOptions, Task<IDbConnection>> handler)
        {
            builder.AddDefaultDBConnectionFactory()
                   .AddResponsibility<DatabaseResponsibilityContext, DelegateDatabaseResponsibility>(new DelegateDatabaseResponsibility(provider, handler));
            return builder;
        }

    }
}
