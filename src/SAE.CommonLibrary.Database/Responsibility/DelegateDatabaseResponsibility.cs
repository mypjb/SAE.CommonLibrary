using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Polly;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.Database.Responsibility
{
    /// <summary>
    /// <see cref="DatabaseResponsibility"/> 的委托实现
    /// </summary>
    public class DelegateDatabaseResponsibility : DatabaseResponsibility
    {
        private readonly Func<string, DBConnectOptions, Task<IDbConnection>> _handler;
        /// <summary>
        /// 使用委托的方式实例化对象
        /// </summary>
        /// <param name="provider">数据库驱动名称</param>
        /// <param name="handler">处理的委托函数</param>
        public DelegateDatabaseResponsibility(string provider, Func<string, DBConnectOptions, Task<IDbConnection>> handler) : base(provider)
        {
            this._handler = handler;
        }
        /// <inheritdoc/>
        protected override async Task HandleCoreAsync(DatabaseResponsibilityContext context)
        {
            // var connection = await this._handler.Invoke(context);
            context.SetInvoke(this._handler);
        }
    }
}
