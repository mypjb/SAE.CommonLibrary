using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    internal abstract class RequestHandlerWrapper
    {
        public abstract Task<object> Invoke(object command);
    }

    internal class RequestHandlerWrapper<TCommand, TResponse> : RequestHandlerWrapper where TCommand : class
    {
        private readonly IEnumerable<ICommandHandler<TCommand, TResponse>> _handlers;

        public RequestHandlerWrapper(IServiceProvider serviceProvider)
        {
            this._handlers = serviceProvider.GetServices<ICommandHandler<TCommand, TResponse>>();



            if (this._handlers == null || !this._handlers.Any())
            {
                var provider = serviceProvider.GetService<IProxyCommandHandlerProvider>();
                if (provider != null)
                    this._handlers = new[] { new DelegateCommandHandlerWrapper<TCommand,TResponse>(provider) };
            }
        }

        public override async Task<object> Invoke(object command)
        {
            object result = null;
            TCommand arg = (TCommand)command;
            foreach (var handler in this._handlers)
            {
                result = await handler.Handle(arg);
            }
            return result;
        }

        private class DelegateCommandHandlerWrapper<TDelegateCommand, TDelegateResponse> : ICommandHandler<TDelegateCommand, TDelegateResponse> where TDelegateCommand : class
        {
            private readonly IProxyCommandHandlerProvider provider;

            public DelegateCommandHandlerWrapper(IProxyCommandHandlerProvider provider)
            {
                this.provider = provider;
            }

            public async Task<TDelegateResponse> Handle(TDelegateCommand command)
            {
                var handler = await this.provider.Get<TDelegateCommand, TDelegateResponse>();
                return await handler.Handle(command);
            }
        }
    }
}
