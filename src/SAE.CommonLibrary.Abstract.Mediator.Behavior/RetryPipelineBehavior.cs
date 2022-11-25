using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;

namespace SAE.CommonLibrary.Abstract.Mediator.Behavior
{
    /// <summary>
    /// 重试管道的配置选项
    /// </summary>
    public class RetryPipelineBehaviorOptions
    {
        /// <summary>
        /// 配置节
        /// </summary>
        public const string Option = "mediator:pipeline:retry";
        /// <summary>
        /// 重试次数
        /// </summary>
        /// <value></value>
        public int Num { get; set; }
    }
    /// <summary>
    /// 拦截请求，当触发异常时，将会触发重试规则。重试设置请参考:
    /// <see cref="RetryPipelineBehaviorOptions"/>
    /// </summary>
    /// <inheritdoc/>
    public class RetryPipelineBehavior<TCommand> : IPipelineBehavior<TCommand> where TCommand : class
    {
        private readonly ILogging _logging;
        private readonly IOptionsSnapshot<RetryPipelineBehaviorOptions> _options;

        public RetryPipelineBehavior(ILogging<RetryPipelineBehavior<TCommand>> logging, IOptionsSnapshot<RetryPipelineBehaviorOptions> options)
        {
            this._options = options;
            this._logging = logging;
        }
        public async Task ExecutionAsync(TCommand command, Func<Task> next)
        {
            await this.ExecutionCoreAsync(command, next);
        }
        private async Task ExecutionCoreAsync(TCommand command, Func<Task> next, int num = 0)
        {
            try
            {
                await next.Invoke();
            }
            catch (Exception ex)
            {
                var optionNum = this._options.Value.Num;
                num++;
                if (num > optionNum)
                {
                    this._logging.Error($"{num}/{optionNum},重试次数用尽，直接触发异常。data:{command.ToJsonString()}");
                    throw ex;
                }
                this._logging.Error(ex, $"命令执行失败'{num}/{optionNum}'次，准备进行重试。data:{command.ToJsonString()}");
                await this.ExecutionCoreAsync(command, next, num);
            }
        }
    }
    /// <summary>
    /// 拦截请求，当触发异常时，将会触发重试规则。重试设置请参考:
    /// <see cref="RetryPipelineBehaviorOptions"/>
    /// </summary>
    /// <inheritdoc/>
    public class RetryPipelineBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse> where TCommand : class
    {
        private readonly ILogging _logging;

        private readonly IOptionsSnapshot<RetryPipelineBehaviorOptions> _options;

        public RetryPipelineBehavior(ILogging<RetryPipelineBehavior<TCommand, TResponse>> logging, IOptionsSnapshot<RetryPipelineBehaviorOptions> options)
        {
            this._options = options;
            this._logging = logging;
        }
        public async Task<TResponse> ExecutionAsync(TCommand command, Func<Task<TResponse>> next)
        {
            return await this.ExecutionCoreAsync(command, next);
        }
        private async Task<TResponse> ExecutionCoreAsync(TCommand command, Func<Task<TResponse>> next, int num = 0)
        {
            try
            {
                return await next.Invoke();
            }
            catch (Exception ex)
            {
                var optionNum = this._options.Value.Num;
                num++;
                if (num > optionNum)
                {
                    this._logging.Error($"{num}/{optionNum},重试次数用尽，直接触发异常。data:{command.ToJsonString()}");
                    throw ex;
                }
                this._logging.Error(ex, $"命令执行失败'{num}/{optionNum}'次，准备进行重试。data:{command.ToJsonString()}");
                return await this.ExecutionCoreAsync(command, next, num);
            }
        }
    }
}