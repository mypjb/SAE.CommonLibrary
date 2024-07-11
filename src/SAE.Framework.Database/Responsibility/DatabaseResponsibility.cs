using SAE.Framework.Abstract.Responsibility;
using SAE.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.Database.Responsibility
{
    /// <summary>
    /// 数据库职责链
    /// </summary>
    public abstract class DatabaseResponsibility : AbstractResponsibility<DatabaseResponsibilityContext>
    {
        /// <summary>
        /// 数据库提供者
        /// </summary>
        protected string Provider { get; }
        /// <summary>
        /// 使用指定的数据库提供者初始化对象
        /// </summary>
        /// <param name="provider"></param>
        public DatabaseResponsibility(string provider)
        {
            this.Provider = provider;
        }

        /// <inheritdoc/>
        public override async Task HandleAsync(DatabaseResponsibilityContext context)
        {
            if (this.HasProvider(context))
                await this.HandleCoreAsync(context);

            await base.HandleAsync(context);
        }
        /// <summary>
        /// 上下文的提供者和当前对象是否匹配
        /// </summary>
        /// <param name="context">上下文</param>
        protected virtual bool HasProvider(DatabaseResponsibilityContext context)
        {
            return context.Options.Provider.Equals(this.Provider, StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>
        /// 核心处理函数
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        protected abstract Task HandleCoreAsync(DatabaseResponsibilityContext context);
    }
}
