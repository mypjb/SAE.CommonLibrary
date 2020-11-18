﻿using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Database;
using SAE.CommonLibrary.Database.Responsibility;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DatabaseDependencyInjectionExtension
    {
        internal const string MYSQL = "mysql";
        private static IServiceCollection AddDBConnectionFactory(this IServiceCollection services)
        {
            services.AddOptions<DBConnectOptions>(DBConnectOptions.Option)
                    .Bind();

            services.TryAddSingleton<IDBConnectionFactory, DBConnectionFactory>();
            return services;
        }
        public static IServiceCollection AddDatabase<TDatabaseResponsibility>(this IServiceCollection services) where TDatabaseResponsibility : DatabaseResponsibility
        {
            services.AddDBConnectionFactory()
                    .AddResponsibility<DatabaseResponsibilityContext, TDatabaseResponsibility>();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services,
            string provider,
            Func<DatabaseResponsibilityContext, Task<IDbConnection>> handler)
        {
            services.AddDBConnectionFactory()
                    .AddResponsibility<DatabaseResponsibilityContext, DelegateDatabaseResponsibility>(new DelegateDatabaseResponsibility(provider, handler));
            return services;
        }

        public static IServiceCollection AddMSSqlDatabase(this IServiceCollection services)
        {
            return services.AddDatabase<MSSqlDatabaseResponsibility>();
        }

        public static IServiceCollection AddMySqlDatabase(this IServiceCollection services)
        {
            return services.AddDatabase(MYSQL, context =>
            {
                return Task.FromResult<IDbConnection>(new MySqlConnector.MySqlConnection(context.Options.ConnectionString));
            });
        }
    }
}
