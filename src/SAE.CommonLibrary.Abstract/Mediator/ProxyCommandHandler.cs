using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    internal class ProxyCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : class
    {
        public Task Handle(TCommand command)
        {
            throw new NotImplementedException();
        }
    }

    internal class ProxyCommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse> where TCommand : class
    {
        Task<TResponse> ICommandHandler<TCommand, TResponse>.Handle(TCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
