using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Abstract.Responsibility;
using SAE.CommonLibrary.Database.Responsibility;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.Database
{
    /// <inheritdoc/>
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
            this._optionsMonitor.OnChange(this.Change);
        }

        /// <summary>
        /// 配置变更
        /// </summary>
        /// <param name="options"></param>
        protected virtual void Change(IEnumerable<DBConnectOptions> dBConnectOptions)
        {
            foreach (var options in dBConnectOptions)
            {
                this.InitialAsync(options).GetAwaiter().GetResult();
            }
        }
        /// <summary>
        /// 初始化操作
        /// </summary>
        protected virtual async Task InitialAsync(DBConnectOptions options)
        {
            if (await this.InitialCheckAsync(options))
            {
                this._logging.Info($"数据库'{options.Name}'不需要进行初始化!");
            }
            else
            {
                this._logging.Info($"准备对数据库'{options.Name}'进行初始化操作!");

                var context = await this.GetContextAsync(options);

                var connectionStrings = options.InitialConnectionString.IsNullOrWhiteSpace() ? options.ConnectionString : options.InitialConnectionString;

                try
                {
                    using (var conn = await context.GetAsync(connectionStrings))
                    {
                        conn.Open();
                        using (var command = conn.CreateCommand())
                        {
                            command.CommandText = options.InitialCommand;
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._logging.Error(ex, $"数据库'{options.Name}'初始化失败,请检查语句或链接是否正常。initialCommand：{options.InitialCommand}");
                }
            }
        }

        /// <summary>
        /// 使用<see cref="DBConnectOptions.InitialDetectionCommand"/>,执行初始化检查，
        /// 返回true已经初始化过，返回false尚未初始化
        /// </summary>
        /// <param name="options">数据库配置对象</param>
        protected virtual async Task<bool> InitialCheckAsync(DBConnectOptions options)
        {
            if (options.InitialCommand.IsNullOrWhiteSpace() ||
                options.InitialDetectionCommand.IsNullOrWhiteSpace())
            {
                this._logging.Info($"该数据库'{options.Name}'不需要进行初始化,如需进行初始化操作，请设置'{nameof(options.InitialCommand)}'、'{nameof(options.InitialDetectionCommand)}'。");
                return true;
            }

            var context = await this.GetContextAsync(options);

            var connectionStrings = options.InitialConnectionString.IsNullOrWhiteSpace() ? options.ConnectionString : options.InitialConnectionString;

            int status = 0;
            try
            {
                using (var conn = await context.GetAsync(connectionStrings))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = options.InitialDetectionCommand;
                        var result = command.ExecuteScalar();
                        int.TryParse(result?.ToString() ?? "0", out status);
                    }
                }
            }
            catch (Exception ex)
            {
                status = 0;
                this._logging.Error(ex, $"在对'{options.Name}'进行数据库检查时出错了,initialDetectionCommand：\r\n{options.InitialDetectionCommand}");
            }
            return status != 0;
        }

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
