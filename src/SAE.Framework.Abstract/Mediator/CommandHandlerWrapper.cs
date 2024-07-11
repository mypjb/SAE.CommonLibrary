using Microsoft.Extensions.DependencyInjection;
using SAE.Framework.Abstract.Mediator.Behavior;
using SAE.Framework.Abstract.Proxy;
using SAE.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.Framework.Abstract.Mediator
{
    /// <summary>
    /// 抽象命令处理装饰器
    /// </summary>
    internal abstract class CommandHandlerWrapper
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">命令</param>
        public abstract Task InvokeAsync(object command);
    }
    /// <summary>
    /// 命令处理包装器
    /// </summary>
    /// <typeparam name="TCommand">命令</typeparam>

    internal class DefaultCommandHandlerWrapper<TCommand> : CommandHandlerWrapper where TCommand : class
    {
        /// <summary>
        /// 命令处理集合
        /// </summary>
        private readonly IEnumerable<ICommandHandler<TCommand>> _handlers;
        /// <summary>
        /// 管道集合
        /// </summary>
        private readonly IEnumerable<IPipelineBehavior<TCommand>> _pipelineBehaviors;
        /// <summary>
        /// 处理程序是否存在
        /// </summary>
        private bool _handlerExist;
        public DefaultCommandHandlerWrapper(IServiceProvider serviceProvider)
        {
            this._handlers = serviceProvider.GetServices<ICommandHandler<TCommand>>();
            this._pipelineBehaviors = serviceProvider.GetServices<IPipelineBehavior<TCommand>>()
                                                     .OrderBy(s => s, OrderComparer.Comparer)
                                                     .ToArray();
#warning 采用其他方式进行调用
            //if (this._handlers == null || !this._handlers.Any())
            //{
            //    var provider= serviceProvider.GetService<IProxyCommandHandlerProvider>();
            //    if (provider != null)
            //        this._handlers = new[] { new DelegateCommandHandlerWrapper<TCommand>(provider) };
            //}
            this._handlerExist = this._handlers.Any();
        }
        ///<inheritdoc/>
        public override async Task InvokeAsync(object command)
        {
            if (!this._handlerExist)
            {
                new SAEException(StatusCodes.ResourcesNotExist, $"'{typeof(ICommandHandler<TCommand>)}' handler not exist");
            }
            TCommand arg = (TCommand)command;

            Func<Task> next = () => this.InvokeCoreAsync(arg);

            var count = this._pipelineBehaviors.Count();

            for (int i = 0; i < this._pipelineBehaviors.Count(); i++)
            {
                var pipelineBehavior = this._pipelineBehaviors.ElementAt(i);
                var pipelineBehaviorWapper = new DefaultPipelineBehaviorWrapper<TCommand>(arg, pipelineBehavior, next);
                next = pipelineBehaviorWapper.Build();
            }

            await next.Invoke();
        }
        /// <summary>
        /// 执行核心
        /// </summary>
        /// <param name="command">命令</param>
        private async Task InvokeCoreAsync(TCommand command)
        {
            foreach (var handler in this._handlers)
            {
                await handler.HandleAsync(command);
            }
        }
#warning 采用其他方式进行调用
        //private class DelegateCommandHandlerWrapper<TDelegateCommand> : ICommandHandler<TDelegateCommand> where TDelegateCommand : class
        //{
        //    private readonly IProxyCommandHandlerProvider provider;

        //    public DelegateCommandHandlerWrapper(IProxyCommandHandlerProvider provider)
        //    {
        //        this.provider = provider;
        //    }

        //    public async Task HandleAsync(TDelegateCommand command)
        //    {
        //        var handler = await this.provider.Get<TDelegateCommand>();
        //        await handler.HandleAsync(command);
        //    }
        //}
    }


}
