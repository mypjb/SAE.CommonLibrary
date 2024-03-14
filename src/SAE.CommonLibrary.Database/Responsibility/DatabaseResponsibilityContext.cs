using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.Database.Responsibility
{
    /// <summary>
    /// 数据库职责链上下文
    /// </summary>
    public class DatabaseResponsibilityContext : Abstract.Responsibility.ResponsibilityContext
    {
        /// <summary>
        /// 使用数据库配置初始化对象
        /// </summary>
        /// <param name="options">数据库链接配置</param>
        public DatabaseResponsibilityContext(DBConnectOptions options)
        {
            Options = options;
        }
        /// <summary>
        /// 数据库配置对象
        /// </summary>
        public DBConnectOptions Options { get; }
        /// <summary>
        /// <see cref="GetAsync(string)"/>委托对象
        /// </summary>
        private Func<string, DBConnectOptions, Task<IDbConnection>> GetInvoke { get; set; }

        /// <summary>
        /// 设置数据库链接
        /// </summary>
        public void SetInvoke(Func<string, DBConnectOptions, Task<IDbConnection>> invoke)
        {
            Assert.Build(invoke)
                  .NotNull($"'{nameof(invoke)}'不能为空");
            this.GetInvoke = invoke;
            this.Success();
        }

        /// <summary>
        /// 获得<see cref="IDbConnection"/>对象
        /// </summary>
        /// <param name="connectionStrings">链接字符串</param>
        public async Task<IDbConnection> GetAsync(string connectionStrings)
        {
            Assert.Build(this.GetInvoke)
                  .NotNull($"请先调用'{nameof(SetInvoke)}'以进行初始化!");
            return await this.GetInvoke(connectionStrings, this.Options);
        }
    }
}
