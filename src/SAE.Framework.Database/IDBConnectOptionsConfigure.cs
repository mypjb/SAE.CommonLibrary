using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAE.Framework.Abstract.Responsibility;
using SAE.Framework.Database.Responsibility;
using SAE.Framework.Extension;
using SAE.Framework.Logging;

namespace SAE.Framework.Database
{
    /// <summary>
    /// 配置<see cref="DBConnectOptions"/>,该接口会在<see cref="DBConnectOptions"/>
    /// 每次初始化的时候调用<see cref="Configure(IEnumerable{DBConnectOptions})"/>
    /// </summary>
    public interface IDBConnectOptionsConfigure
    {
        /// <summary>
        /// 实例化对象后会对<paramref name="options"/>进行二次配置
        /// </summary>
        /// <param name="options">配置对象</param>
        void Configure(IEnumerable<DBConnectOptions> options);
    }

    /// <summary>
    /// <see cref="IDBConnectOptionsConfigure"/> 默认实现
    /// </summary>
    public class DefaultDBConnectOptionsConfigure : IDBConnectOptionsConfigure
    {
        /// <summary>
        /// 职责链对象
        /// </summary>
        private readonly IResponsibility<DatabaseResponsibilityContext> _responsibility;
        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogging _logging;

        /// <summary>
        /// 初始化默认数据库链接工厂
        /// </summary>
        /// <param name="provider">数据库提供对象接口</param>
        /// <param name="logging"></param>
        public DefaultDBConnectOptionsConfigure(IResponsibilityProvider<DatabaseResponsibilityContext> provider,
                                                ILogging<DefaultDBConnectOptionsConfigure> logging)
        {
            this._responsibility = provider.Root;
            this._logging = logging;
        }
        /// <summary>
        /// 配置变更
        /// </summary>
        /// <param name="dBConnectOptions">数据库链接配置集</param>
        public virtual void Configure(IEnumerable<DBConnectOptions> dBConnectOptions)
        {
            if (dBConnectOptions == null || !dBConnectOptions.Any())
            {
                this._logging.Warn("尚未对数据库进行设置!");
                return;
            }
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
            var message =$"数据库'{options.Name}'";
            if (await this.InitialCheckAsync(options))
            {
                this._logging.Info($"{message}不需要进行初始化!");
            }
            else
            {
                this._logging.Info($"准备对{message}进行初始化操作!");

                var context = await this.GetContextAsync(options);

                if (!context.Complete)
                {
                    this._logging.Warn($"无法获取{message}对应的上下文!");
                    return;
                }

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
                    this._logging.Info($"{message},初始化操作完成。");
                }
                catch (Exception ex)
                {
                    this._logging.Error(ex, $"{message}初始化失败,请检查语句或链接是否正常。initialCommand：{options.InitialCommand}");
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

    }
}