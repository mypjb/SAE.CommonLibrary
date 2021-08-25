using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Abstract.Mediator.Behavior;
using SAE.CommonLibrary.Abstract.Proxy;
using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    internal abstract class CommandHandlerWrapper
    {
        public abstract Task InvokeAsync(object command);
    }

    internal class CommandHandlerWrapper<TCommand> : CommandHandlerWrapper where TCommand : class
    {
        private readonly IEnumerable<ICommandHandler<TCommand>> _handlers;
        private readonly IEnumerable<IPipelineBehavior<TCommand>> _pipelineBehaviors;
        public CommandHandlerWrapper(IServiceProvider serviceProvider)
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
        }

        public override async Task InvokeAsync(object command)
        {
            TCommand arg = (TCommand)command;

            Func<Task> next = () => this.InvokeCoreAsync(arg);

            var count = this._pipelineBehaviors.Count();

            for (int i = 0; i < this._pipelineBehaviors.Count(); i++)
            {
                var pipelineBehavior = this._pipelineBehaviors.ElementAt(i);
                var pipelineBehaviorWapper = new PipelineBehaviorWapper<TCommand>(arg, pipelineBehavior, next);
                next = pipelineBehaviorWapper.Build();
            }

            await next.Invoke();
        }

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
