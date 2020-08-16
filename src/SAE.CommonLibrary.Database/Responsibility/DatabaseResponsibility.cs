using SAE.CommonLibrary.Abstract.Responsibility;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Database.Responsibility
{
    /// <summary>
    /// database responsibility
    /// </summary>
    public abstract class DatabaseResponsibility : AbstractResponsibility<DatabaseResponsibilityContext>
    {
        /// <summary>
        /// database provider
        /// </summary>
        protected string Provider { get; }
        public DatabaseResponsibility(string provider)
        {
            this.Provider = provider;
        }
        /// <summary>
        /// override <seealso cref="AbstractResponsibility.HandleAsync(TContext)"/> method
        /// </summary>
        /// <param name="context"><seealso cref="DatabaseResponsibilityContext"/></param>
        /// <returns></returns>
        public override async Task HandleAsync(DatabaseResponsibilityContext context)
        {
            if (this.HasProvider(context))
                await this.HandleCoreAsync(context);

            await base.HandleAsync(context);

            Assert.Build(!context.Complete && this.Responsibility == null)
                  .False($"I won't support it '{context.Options.Provider}' provider");
        }
        /// <summary>
        /// <paramref name="context"/> provider is <seealso cref="Provider"/>?
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual bool HasProvider(DatabaseResponsibilityContext context)
        {
            return context.Options.Provider.Equals(this.Provider, StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>
        /// handler core
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract Task HandleCoreAsync(DatabaseResponsibilityContext context);
    }
}
