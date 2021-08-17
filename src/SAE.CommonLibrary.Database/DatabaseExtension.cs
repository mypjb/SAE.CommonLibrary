using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Database
{
    public static class DatabaseExtension
    {
        private const string DefaultName = "Default";
        public static Task<IDbConnection> GetAsync(this IDBConnectionFactory factory)
        {
            return factory.GetAsync(DefaultName);
        }

        public static IDbConnection Get(this IDBConnectionFactory factory)
        {
            return factory.Get(DefaultName);
        }

        public static IDbConnection Get(this IDBConnectionFactory factory,string name)
        {
            return factory.GetAsync(name).GetAwaiter().GetResult();
        }
    }
}
