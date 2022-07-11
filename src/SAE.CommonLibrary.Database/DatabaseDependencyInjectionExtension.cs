using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Database;
using SAE.CommonLibrary.Database.Responsibility;
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class DatabaseDependencyInjectionExtension
    {
        internal const string MYSQL = "mysql";
        /// <summary>
        /// add default db connection <see cref="DefaultDBConnectionFactory"/>
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
        /// use <see cref="ScopeDBConnectionFactory"/> imp <see cref="IDBConnectionFactory"/>
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddScopeDatabaseFactory(this IServiceCollection services)
        {
            services.TryAddSingleton<IDBConnectionFactory, ScopeDBConnectionFactory>();
            services.AddOptions<List<DBConnectOptions>>()
                    .Bind(DBConnectOptions.Option);
            return services;
        }

        /// <summary>
        /// add default provider
        /// </summary>
        /// <param name="services"></param>
        /// <typeparam name="TDatabaseResponsibility"></typeparam>
        public static IServiceCollection AddDatabase<TDatabaseResponsibility>(this IServiceCollection services) where TDatabaseResponsibility : DatabaseResponsibility
        {
            services.AddDefaultDBConnectionFactory()
                    .AddResponsibility<DatabaseResponsibilityContext, TDatabaseResponsibility>();
            return services;
        }

        /// <summary>
        /// add provider
        /// </summary>
        /// <param name="services"></param>
        /// <param name="provider"></param>
        /// <param name="handler"></param>
        public static IServiceCollection AddDatabase(this IServiceCollection services,
            string provider,
            Func<DatabaseResponsibilityContext, Task<IDbConnection>> handler)
        {
            services.AddDefaultDBConnectionFactory()
                    .AddResponsibility<DatabaseResponsibilityContext, DelegateDatabaseResponsibility>(new DelegateDatabaseResponsibility(provider, handler));
            return services;
        }

        /// <summary>
        /// add mssql provider
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddMSSqlDatabase(this IServiceCollection services)
        {
            return services.AddDatabase<MSSqlDatabaseResponsibility>();
        }
        /// <summary>
        /// add mysql provider
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddMySqlDatabase(this IServiceCollection services)
        {
            return services.AddDatabase(MYSQL, context =>
            {
                return Task.FromResult<IDbConnection>(new MySqlConnector.MySqlConnection(context.Options.ConnectionString));
            });
        }
    }
}
