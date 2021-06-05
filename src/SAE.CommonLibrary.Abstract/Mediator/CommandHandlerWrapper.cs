using Microsoft.Extensions.DependencyInjection;
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

        public CommandHandlerWrapper(IServiceProvider serviceProvider)
        {
            this._handlers = serviceProvider.GetServices<ICommandHandler<TCommand>>();

            if (this._handlers == null || !this._handlers.Any())
            {
                var provider= serviceProvider.GetService<IProxyCommandHandlerProvider>();
                if (provider != null)
                    this._handlers = new[] { new DelegateCommandHandlerWrapper<TCommand>(provider) };
            }
        }

        public override async Task InvokeAsync(object command)
        {
            TCommand arg = (TCommand)command;
            await _handlers.ForEachAsync(async handler => await handler.HandleAsync(arg));
        }

        private class DelegateCommandHandlerWrapper<TDelegateCommand> : ICommandHandler<TDelegateCommand> where TDelegateCommand : class
        {
            private readonly IProxyCommandHandlerProvider provider;

            public DelegateCommandHandlerWrapper(IProxyCommandHandlerProvider provider)
            {
                this.provider = provider;
            }

            public async Task HandleAsync(TDelegateCommand command)
            {
                var handler = await this.provider.Get<TDelegateCommand>();
                await handler.HandleAsync(command);
            }
        }
    }

    
}
