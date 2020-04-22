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

            var provider = serviceProvider.GetService<IProxyCommandHandlerProvider>();

            if (provider != null && !this._handlers.Any())
            {
                this._handlers = new[] { provider.Get<TCommand, TResponse>() };
            }
        }

        public override async Task<object> Invoke(object command)
        {
            TCommand arg = (TCommand)command;
            foreach (var handler in this._handlers)
            {
                return await handler.Handle(arg);
            }
            return null;
        }
    }
}
