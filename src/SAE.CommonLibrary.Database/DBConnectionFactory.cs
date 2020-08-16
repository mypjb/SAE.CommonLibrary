using SAE.CommonLibrary.Abstract.Responsibility;
using SAE.CommonLibrary.Database.Responsibility;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Database
{
    public class DBConnectionFactory : IDBConnectionFactory
    {
        private readonly IEnumerable<DBConnectOptions> _options;

        private readonly IResponsibility<DatabaseResponsibilityContext> _responsibility;

        public DBConnectionFactory(IEnumerable<DBConnectOptions> options,
               IResponsibilityProvider<DatabaseResponsibilityContext> provider)
        {
            this._options = options;
            this._responsibility = provider.Root;
        }

        public async Task<IDbConnection> GetAsync(string name)
        {
            Assert.Build(name)
                  .NotNullOrWhiteSpace($"'name' cannot be empty");

            var option = this._options.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            Assert.Build(option)
                  .NotNull($"'{name}' database connection not exist");

            var context = new DatabaseResponsibilityContext(option);

            await this._responsibility.HandleAsync(context);

            return context.DbConnection;
        }
    }
}
