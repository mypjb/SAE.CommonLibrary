using Polly;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Database.Responsibility
{
    /// <summary>
    /// Delegate <seealso cref="DatabaseResponsibility"/> Implementation
    /// </summary>
    public class DelegateDatabaseResponsibility : DatabaseResponsibility
    {
        private readonly Func<DatabaseResponsibilityContext, Task<IDbConnection>> _handler;

        public DelegateDatabaseResponsibility(string provider, Func<DatabaseResponsibilityContext, Task<IDbConnection>> handler) : base(provider)
        {
            this._handler = handler;
        }
        protected override async Task HandleCoreAsync(DatabaseResponsibilityContext context)
        {
            var connection = await this._handler.Invoke(context);
            context.SetDbConnection(connection);
        }
    }
}
