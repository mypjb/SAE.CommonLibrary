using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Abstract.Responsibility;
using SAE.CommonLibrary.Database.Responsibility;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.Database
{
    public class DefaultDBConnectionFactory : IDBConnectionFactory
    {
        protected IEnumerable<DBConnectOptions> Options
        {
            get
            {
                return this._optionsMonitor.CurrentValue;
            }
        }

        private readonly IResponsibility<DatabaseResponsibilityContext> _responsibility;
        private readonly IOptionsMonitor<List<DBConnectOptions>> _optionsMonitor;

        public DefaultDBConnectionFactory(IOptionsMonitor<List<DBConnectOptions>> optionsMonitor,
                                          IResponsibilityProvider<DatabaseResponsibilityContext> provider)
        {
            this._responsibility = provider.Root;
            this._optionsMonitor = optionsMonitor;
        }

        public async Task<IDbConnection> GetAsync(string name)
        {
            Assert.Build(name)
                  .NotNullOrWhiteSpace($"'name' cannot be empty");

            var options = await this.GetOptionsAsync(name);

            Assert.Build(options)
                  .NotNull($"'{name}' database connection not exist");

            return await this.GetCoreAsync(options);
        }
        /// <summary>
        /// get <seealso cref="DBConnectOptions"/>
        /// </summary>
        /// <param name="name"></param>
        protected virtual Task<DBConnectOptions> GetOptionsAsync(string name)
        {
            var options = this.Options.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(options);
        }

        /// <summary>
        /// get <seealso cref="IDbConnection"/>
        /// </summary>
        /// <param name="options"></param>

        protected virtual async Task<IDbConnection> GetCoreAsync(DBConnectOptions options)
        {
            var context = new DatabaseResponsibilityContext(options);

            await this._responsibility.HandleAsync(context);

            return context.DbConnection;
        }
    }
}
