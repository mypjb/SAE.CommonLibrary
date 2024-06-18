using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SAE.Framework.Abstract.Responsibility;
using SAE.Framework.Database.Responsibility;
using SAE.Framework.Extension;
using SAE.Framework.Logging;

namespace SAE.Framework.Database
{

    /// <summary>
    /// 默认<see cref="IDBConnectionFactory"/>实现
    /// </summary>
    public class DefaultDBConnectionFactory : IDBConnectionFactory
    {
        /// <summary>
        /// 配置集合
        /// </summary>
        /// <value></value>
        protected IEnumerable<DBConnectOptions> Options
        {
            get
            {
                return this._optionsMonitor.CurrentValue;
            }
        }
        /// <summary>
        /// 职责链对象
        /// </summary>
        private readonly IResponsibility<DatabaseResponsibilityContext> _responsibility;
        /// <summary>
        /// 配置监控对象
        /// </summary>
        private readonly IOptionsMonitor<List<DBConnectOptions>> _optionsMonitor;
        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogging<DefaultDBConnectionFactory> _logging;

        /// <summary>
        /// 初始化默认数据库链接工厂
        /// </summary>
        /// <param name="optionsMonitor">配置监控对象</param>
        /// <param name="provider">数据库提供对象接口</param>
        /// <param name="logging"></param>
        public DefaultDBConnectionFactory(IOptionsMonitor<List<DBConnectOptions>> optionsMonitor,
                                          IResponsibilityProvider<DatabaseResponsibilityContext> provider,
                                          ILogging<DefaultDBConnectionFactory> logging)
        {
            this._responsibility = provider.Root;
            this._optionsMonitor = optionsMonitor;
            this._logging = logging;
        }

        /// <inheritdoc/>
        public async Task<IDbConnection> GetAsync(string name)
        {
            Assert.Build(name)
                  .NotNullOrWhiteSpace($"'name' 不能为空");

            var options = await this.GetOptionsAsync(name);

            Assert.Build(options)
                  .NotNull($"'{name}' 数据库不存在");

            return await this.GetCoreAsync(options);
        }
        /// <summary>
        /// 获得<see cref="DBConnectOptions"/>核心处理函数
        /// </summary>
        /// <param name="name">配置名称</param>
        protected virtual Task<DBConnectOptions> GetOptionsAsync(string name)
        {
            var options = this.Options.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(options);
        }

        /// <summary>
        /// 获得数据上下文
        /// </summary>
        /// <param name="options">配置对象</param>
        protected virtual async Task<DatabaseResponsibilityContext> GetContextAsync(DBConnectOptions options)
        {
            var context = new DatabaseResponsibilityContext(options);

            await this._responsibility.HandleAsync(context);
            return context;
        }

        /// <summary>
        /// 获得<see cref="IDbConnection"/>核心处理函数
        /// </summary>
        /// <param name="options">配置对象</param>

        protected virtual async Task<IDbConnection> GetCoreAsync(DBConnectOptions options)
        {
            var context = await this.GetContextAsync(options);

            return await context.GetAsync(options.ConnectionString);
        }
    }
}
