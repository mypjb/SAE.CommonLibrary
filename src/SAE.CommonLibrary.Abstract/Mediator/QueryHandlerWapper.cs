using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
        private readonly IEnumerable<IRequestHandler<TCommand, TResponse>> _handlers;

        public RequestHandlerWrapper(IServiceProvider serviceProvider)
        {
            this._handlers = serviceProvider.GetServices<IRequestHandler<TCommand, TResponse>>();
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
