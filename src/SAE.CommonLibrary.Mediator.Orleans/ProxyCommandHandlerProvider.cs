using SAE.CommonLibrary.Abstract.Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    public class ProxyCommandHandlerProvider : IProxyCommandHandlerProvider
    {
        public ProxyCommandHandlerProvider()
        {

        }
        public ICommandHandler<TCommand> Get<TCommand>() where TCommand : class
        {
            throw new NotImplementedException();
        }

        public ICommandHandler<TCommand, TResponse> Get<TCommand, TResponse>() where TCommand : class
        {
            throw new NotImplementedException();
        }
    }
}
