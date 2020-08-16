using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Database.Responsibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Database
{
    public static class DatabaseDependencyInjectionExtension
    {
        private static IServiceCollection AddDBConnectionFactory(this IServiceCollection services)
        {
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
            return services.AddDatabase("MySql.Data.MySqlClient", context =>
            {
                return Task.FromResult<IDbConnection>(new MySql.Data.MySqlClient.MySqlConnection(context.Options.ConnectionString));
            });
        }
    }
}
