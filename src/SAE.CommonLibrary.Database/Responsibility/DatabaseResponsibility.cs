using SAE.CommonLibrary.Abstract.Responsibility;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Database.Responsibility
{
    /// <summary>
    /// 数据库职责链
    /// </summary>
    /// <inheritdoc/>
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
    
        public override async Task HandleAsync(DatabaseResponsibilityContext context)
        {
            if (this.HasProvider(context))
                await this.HandleCoreAsync(context);

            await base.HandleAsync(context);

            Assert.Build(!context.Complete && this.Responsibility == null)
                  .False($"不支持这个'{context.Options.Provider}'数据库驱动");
        }
        /// <summary>
        /// 上下文的提供者和当前对象是否匹配
        /// </summary>
        /// <param name="context"></param>
        protected virtual bool HasProvider(DatabaseResponsibilityContext context)
        {
            return context.Options.Provider.Equals(this.Provider, StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>
        /// 核心处理函数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract Task HandleCoreAsync(DatabaseResponsibilityContext context);
    }
}
