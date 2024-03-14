using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Abstract.Mediator.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    /// <summary>
    /// 抽象请求处理程序包装器
    /// </summary>
    internal abstract class RequestHandlerWrapper
    {
        
           public abstract Task<object> InvokeAsync(object command);
    }
    /// <summary>
    /// 默认请求处理包装器
    /// </summary>
    /// <typeparam name="TCommand">命令</typeparam>
    /// <typeparam name="TResponse">响应</typeparam>
    internal class DefaultRequestHandlerWrapper<TCommand, TResponse> : RequestHandlerWrapper where TCommand : class
    {
        /// <summary>
        /// 处理集合
        /// </summary>
        private readonly IEnumerable<ICommandHandler<TCommand, TResponse>> _handlers;
        /// <summary>
        /// 行为管道集合
        /// </summary>
        private readonly IEnumerable<IPipelineBehavior<TCommand, TResponse>> _pipelineBehaviors;
        /// <summary>
        /// 处理程序是否存在
        /// </summary>
        private readonly bool _handlerExist;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        public DefaultRequestHandlerWrapper(IServiceProvider serviceProvider)
        {
            this._pipelineBehaviors = serviceProvider.GetServices<IPipelineBehavior<TCommand, TResponse>>()
                                                   .OrderBy(s => s, OrderComparer.Comparer)
                                                   .ToArray();

            this._handlers = serviceProvider.GetServices<ICommandHandler<TCommand, TResponse>>()
                                            .OrderByDescending(s => s, OrderComparer.Comparer)
                                            .ToArray();

#warning 采用其他方式进行调用
            //if (this._handlers == null || !this._handlers.Any())
            //{
            //    var provider = serviceProvider.GetService<IProxyCommandHandlerProvider>();
            //    if (provider != null)
            //        this._handlers = new[] { new DelegateCommandHandlerWrapper<TCommand, TResponse>(provider) };
            //}
            this._handlerExist = this._handlers.Any();
        }
        /// <inheritdoc/>
        public override async Task<object> InvokeAsync(object command)
        {
            if (!this._handlerExist)
            {
                new SAEException(StatusCodes.ResourcesNotExist, $"'{typeof(ICommandHandler<TCommand, TResponse>)}' handler not exist");
            }
            TCommand arg = (TCommand)command;

            Func<Task<TResponse>> next = () => this.InvokeCoreAsync(arg);

            var count = this._pipelineBehaviors.Count();

            for (int i = 0; i < this._pipelineBehaviors.Count(); i++)
            {
                var pipelineBehavior = this._pipelineBehaviors.ElementAt(i);
                var pipelineBehaviorWapper = new DefaultPipelineBehaviorWrapper<TCommand, TResponse>(arg, pipelineBehavior, next);
                next = pipelineBehaviorWapper.Build();
            }

            return await next.Invoke();
        }
        /// <summary>
        /// 核心执行函数
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>响应</returns>
        private async Task<TResponse> InvokeCoreAsync(TCommand command)
        {
            TResponse result = default(TResponse);
            foreach (var handler in this._handlers)
            {
                result = await handler.HandleAsync(command);
            }

            return result;
        }
#warning 采用其他方式进行调用
        //private class DelegateCommandHandlerWrapper<TDelegateCommand, TDelegateResponse> : ICommandHandler<TDelegateCommand, TDelegateResponse> where TDelegateCommand : class
        //{
        //    private readonly IProxyCommandHandlerProvider provider;

        //    public DelegateCommandHandlerWrapper(IProxyCommandHandlerProvider provider)
        //    {
        //        this.provider = provider;
        //    }

        //    public async Task<TDelegateResponse> HandleAsync(TDelegateCommand command)
        //    {
        //        var handler = await this.provider.Get<TDelegateCommand, TDelegateResponse>();
        //        return await handler.HandleAsync(command);
        //    }
        //}
    }
}
